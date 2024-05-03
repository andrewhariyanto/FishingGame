// Fish size types
public enum Fish{
    S,
    M,
    L
}

// Fish color types
public enum Color {
    Red,
    Blue,
    Green
}

// Class to initialize and update forecast
public class Forecast {

    private int _totalFish = 0;
    private Dictionary<Fish, int> _fishCount = new Dictionary<Fish, int>();
    private Dictionary<Color, float> _colorPercentage = new Dictionary<Color, float>();
    private int[] _proportion = {40, 35, 25};

    // Initializes a random forecast given a minimum and maximum number of fish
    public void GenerateForecast(int poolMin, int poolMax){
        Random rand = new Random();

        float total = rand.Next(poolMin, poolMax + 1);
        float[] proportions = GenerateRandom();
        float[] colors = GenerateRandom();

        for (int i = 0; i < 3; i++){
            int tempFish = (int) MathF.Round(total*proportions[i]);
            _fishCount[Enum.GetValues<Fish>()[i]] = tempFish;
            _totalFish += tempFish;
            _colorPercentage[Enum.GetValues<Color>()[i]] = colors[i];
        }

        PrintForecast(_fishCount, _colorPercentage);
    }

    // Prints the forecast
    private static void PrintForecast(Dictionary<Fish, int> fishCount, Dictionary<Color, float> colorPercentage){
        Console.WriteLine("Current Forecast");
        Console.WriteLine("Fishes -- Small: " + fishCount[Fish.S] + "\t Medium: " + fishCount[Fish.M] + "\t Large: " + fishCount[Fish.L]);
        Console.WriteLine("Colors -- Red: " + String.Format("{0:P0}", colorPercentage[Color.Red]) + 
            "\t Blue: " + String.Format("{0:P0}", colorPercentage[Color.Blue]) +
            "\t Green: " + String.Format("{0:P0}", colorPercentage[Color.Green]));
    }

    // Used to determine the proportion of different sized fish - option for generating random fish population
    private float[] GenerateProportions(){
        Random rand = new Random();
        float[] results = new float[3];
        int max = 100;
        int accum = 0;
        
        for (int i = 0; i < 2; i++){
            int variance = rand.Next(-5, 6);
            int temp = _proportion[i] + variance;
            accum += temp;
            results[i] = temp/100f;
        }

        results[2] = (max-accum)/100f;

        return results;
    }

    // Random number generator for determining proportions and colors
    private float[] GenerateRandom(){
        Random rand = new Random();
        float[] results = new float[3];
        int max = 100;
        int accum = 0;

        for(int i = 0; i < 2; i++){
            int temp = rand.Next(0, max);
            max -= temp;
            accum += temp;
            results[i] = temp/100f;
        }

        results[2] = max/100f;

        return results;
    }

    // Updates the forecast when a fish is fished
    public void UpdateForecast(Fish fish, Color color){
        UpdateColor(color);
        _fishCount[fish] --;
        _totalFish--;
        PrintForecast(_fishCount, _colorPercentage);
    }

    // Helper function for updating forecast
    private void UpdateColor(Color color){
        for(int i = 0; i < 3; i++){
            float before = _totalFish * _colorPercentage[Enum.GetValues<Color>()[i]];
            if(color == Enum.GetValues<Color>()[i]){
                before --;
            }
            _colorPercentage[Enum.GetValues<Color>()[i]] = before / (_totalFish-1);
        }
    }

    // Getters
    public int GetSmallFish(){
        return _fishCount[Fish.S];
    }

    public int GetMediumFish(){
        return _fishCount[Fish.M];
    }

    public int GetLargeFish(){
        return _fishCount[Fish.L];
    }

    public int GetFishByCategory(Fish type){
        return _fishCount[type];
    }

    public float GetProbabilityByCategorty(Color color){
        return _colorPercentage[color];
    }
    public float GetRedProbability(){
        return _colorPercentage[Color.Red];
    }

    public float GetBlueProbability(){
        return _colorPercentage[Color.Blue];
    }

    public float GetGreenProbability(){
        return _colorPercentage[Color.Green];
    }

    public float GetTotal(){
        return _totalFish;
    }
    
}