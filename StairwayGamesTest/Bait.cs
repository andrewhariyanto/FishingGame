public class Bait {
    private Dictionary<Color, int> _baitCosts = new Dictionary<Color, int>(){
        { Color.Red, 1},
        { Color.Blue, 2},
        { Color.Green, 3}
    };

    private Dictionary<Color, int> _baitSupply = new Dictionary<Color, int>();

    public Color DetermineBait(Forecast forecast, Fish type, FishSales fishSales){
        float expectedRed = _baitSupply[Color.Red] > 0 ? forecast.GetRedProbability() * fishSales.GetExpectedValue((type, Color.Red)) : 0;
        float expectedBlue = _baitSupply[Color.Blue] > 0 ? forecast.GetBlueProbability() * fishSales.GetExpectedValue((type, Color.Blue)) : 0;
        float expectedGreen = _baitSupply[Color.Green] > 0 ? forecast.GetGreenProbability() * fishSales.GetExpectedValue((type, Color.Green)) : 0;

        return Max(expectedRed, expectedBlue, expectedGreen);
    }

    private Color Max(float r, float b, float g){
        return r < b ? (b < g ? Color.Green : Color.Blue) : (r < g ? Color.Green : Color.Red);
    }

    // Uses a greedy algorithm based on the highest revenue per gold spent to determine the number of baits
    public void BuyBait(Forecast forecast, Fish fishType, FishSales fishSales, int gold){
        (Color, int, float)[] results = new (Color, int, float)[3];
        for (int i = 0; i < 3; i++){
            Color c = Enum.GetValues<Color>()[i];
            int expectedVal = CalculateExpectedBait(forecast.GetFishByCategory(fishType), forecast.GetProbabilityByCategorty(c));
            float revPerGold = CalculateRevPerGoldSpend(expectedVal, (fishType, c), fishSales);
            results[i] = (c, expectedVal, revPerGold);
        }

        Array.Sort(results, new Comparison<(Color, int, float)>((a, b) => b.Item3.CompareTo(a.Item3)));

        // Determines the number of expected baits to buy and add to dictionary
        (Color, int)[] baitBought = new (Color, int)[3];
        for(int i = 0 ; i < results.Length; i++){
            (int, int) transaction = CalculatePossibleBait(gold, results[i].Item2, results[i].Item1);
            baitBought[i] = (results[i].Item1, transaction.Item1);
            _baitSupply[results[i].Item1] = baitBought[i].Item2;
            gold = transaction.Item2;
        }

        // Spends all the gold left on bait based on the proportion of baits calculated in expected number of baits needed
        (Color, float)[] proportions = CalculateProportion(baitBought);
        int goldSpent = 0;
        for(int i = 0; i < 3; i++){
            int extras = ExhaustFunds(gold, proportions[i]);
            goldSpent += extras * _baitCosts[proportions[i].Item1];
            _baitSupply[proportions[i].Item1] += extras;
        }
        gold -= goldSpent;

        // Leftovers for the most priority bait
        for(int i = 0; i < 3; i++){
            int expectedBait = gold/_baitCosts[results[i].Item1];
            (int, int) transaction = CalculatePossibleBait(gold, expectedBait, results[i].Item1);
            _baitSupply[results[i].Item1] += transaction.Item1;
            gold = transaction.Item2;
        }
        
        PrintReceipt();
    }

    // Print function to show status of buying baits
    private void PrintReceipt(){
        Console.WriteLine("Now Buying: " + "Red bait " + _baitSupply[Color.Red] + " for " + _baitSupply[Color.Red] *_baitCosts[Color.Red]
            + " gold \t Blue bait " + _baitSupply[Color.Blue] + " for " + _baitSupply[Color.Blue] * _baitCosts[Color.Blue]
            + " gold \t Green bait " + _baitSupply[Color.Green] + " for " + _baitSupply[Color.Green] * _baitCosts[Color.Green] + " gold");
    }

    // Helper function to use up all gold
    private int ExhaustFunds(int gold, (Color, float) proportions){
        return (int) MathF.Floor(gold * proportions.Item2 / _baitCosts[proportions.Item1]);
    }

    // Helper function to calculate the possible bait accoridng to expected bait and color
    private (int, int) CalculatePossibleBait(int gold, int expectedBait, Color color){
        int baitBought = expectedBait;
        int expectedCost = expectedBait * _baitCosts[color];
        while(expectedCost > gold){
            baitBought--;
            expectedCost -= _baitCosts[color];
        }
        gold -= expectedCost;
        return (baitBought, gold);
    }

    // Helper function to calculate the proportion for how much gold is being allocated for each bait type
    private (Color, float)[] CalculateProportion((Color, int)[] baits){
        float sum = 0;
        Array.ForEach(baits, (a) => sum += (a.Item2 * _baitCosts[a.Item1]));
        (Color, float)[] result = new (Color, float)[3];
        for(int i = 0; i < baits.Length; i++){
            result[i] = (baits[i].Item1, baits[i].Item2 * _baitCosts[baits[i].Item1]/sum);
        }
        return result;
    }

    // Helper Function to calculate the expected number of baits needed
    private int CalculateExpectedBait(int totalFish, float probability){
        return (int) MathF.Round(totalFish * probability);
    }

    // Helper function to calculate the expected revenue per gold spent for each bait type
    private float CalculateRevPerGoldSpend(int fish, (Fish, Color) info, FishSales fishSales){
        return fish * fishSales.GetExpectedValue(info) / _baitCosts[info.Item2];
    }

    // Getters and Setters
    public void SetRedBait(int num){
        _baitSupply[Color.Red] = num;
    }
    public void SetBlueBait(int num){
        _baitSupply[Color.Blue] = num;
    }
    public void SetGreenBait(int num){
        _baitSupply[Color.Green] = num;
    }

    public int GetRedBait(){
        return _baitSupply[Color.Red];
    }
    public int GetBlueBait(){
        return _baitSupply[Color.Blue];
    }
    public int GetGreenBait(){
        return _baitSupply[Color.Green];
    }

    public int CalculateTotalCost(int[] bait){
        return bait[0] * _baitCosts[Color.Red] + bait[1] * _baitCosts[Color.Blue] + bait[2] * _baitCosts[Color.Green];
    }

    public void UpdateBait(Color chosenBait){
        _baitSupply[chosenBait] --;
    }
}