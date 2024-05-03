public class Pole {
    private Fish type;
    private Dictionary<Fish, int> prices = new Dictionary<Fish, int>(){
        { Fish.S, 5},
        { Fish.M, 10},
        { Fish.L, 15}
    };

    (float, float, float) expectedFish;

    // Calculates the expected revenue per size category
    public void GenerateExpectedFish(Forecast forecast, FishSales fishSales){
        float expectedValueS = forecast.GetSmallFish() * CalculateExpected(forecast, Fish.S, fishSales);
        float expectedValueM = forecast.GetMediumFish() * CalculateExpected(forecast, Fish.M, fishSales);
        float expectedValueL = forecast.GetLargeFish() * CalculateExpected(forecast, Fish.L, fishSales);

        Console.WriteLine("Expected S Fish Revenue: " + expectedValueS.ToString("#.##") + "\tExpected M Fish Revenue: " 
            + expectedValueM.ToString("#.##") + "\tExpected L Fish Revenue: " + expectedValueL.ToString("#.##"));

        expectedFish = (expectedValueS, expectedValueM, expectedValueL);
    }

    // Recommends the pole to use by choosing the best expected revenue
    public Fish RecommendPole(){
        type = Max(expectedFish.Item1, expectedFish.Item2, expectedFish.Item3);
        return type;
    }

    // Sets the pole
    public int BuyPole(Fish type){
        this.type = type;
        PrintPoleBuy();
        return prices[type];
    }
    
    // helper to calculate expected revenue
    private float CalculateExpected(Forecast forecast, Fish fish, FishSales fishSales){
        return forecast.GetRedProbability() * fishSales.GetExpectedValue((fish, Color.Red))
            + forecast.GetBlueProbability() * fishSales.GetExpectedValue((fish, Color.Blue))
            + forecast.GetGreenProbability() * fishSales.GetExpectedValue((fish, Color.Green));
    }

    // Print function to show what pole is being bought
    public void PrintPoleBuy(){
        Console.WriteLine("Now Buying: " + type.ToString() + " pole");
    }

    // Helper function to determine max of three values
    private Fish Max(float s, float m, float l){
        return s < m ? (m < l ? Fish.L : Fish.M) : (s < l ? Fish.L : Fish.S);
    }

    // Getter
    public Fish GetPoleType(){
        return type;
    }
}