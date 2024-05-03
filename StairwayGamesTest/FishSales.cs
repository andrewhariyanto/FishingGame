public class FishSales{
    private Dictionary<(Fish, Color), float> _expectedSales = new Dictionary<(Fish, Color), float>() {
        {(Fish.S, Color.Red), 3},
        {(Fish.M, Color.Red), 7.5f},
        {(Fish.L, Color.Red), 12.5f},
        {(Fish.S, Color.Blue), 4},
        {(Fish.M, Color.Blue), 9},
        {(Fish.L, Color.Blue), 14},
        {(Fish.S, Color.Green), 5},
        {(Fish.M, Color.Green), 10},
        {(Fish.L, Color.Green), 15},
    };

    private Dictionary<(Fish, Color), (int, int)> _sales = new Dictionary<(Fish, Color), (int, int)>() {
        {(Fish.S, Color.Red), (1, 6)},
        {(Fish.M, Color.Red), (5, 11)},
        {(Fish.L, Color.Red), (10, 16)},
        {(Fish.S, Color.Blue), (3, 6)},
        {(Fish.M, Color.Blue), (8, 11)},
        {(Fish.L, Color.Blue), (13, 16)},
        {(Fish.S, Color.Green), (5, 6)},
        {(Fish.M, Color.Green), (10, 11)},
        {(Fish.L, Color.Green), (15, 16)},
    };

    // Returns the expected revenue for a specified size and colored fish
    public float GetExpectedValue((Fish, Color) info){
        return _expectedSales[info];
    }

    // Returns the actual revenue for a specified size and colored fish
    public int CalculateActual((Fish, Color) info){
        Random rand = new Random();
        (int, int) pair = _sales[info];
        return rand.Next(pair.Item1, pair.Item2);
    }
}