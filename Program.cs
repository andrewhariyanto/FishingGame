using System;

class Program {
    static void Main(){
        Forecast forecast = new Forecast();
        FishSales fishSales = new FishSales();
        Pole pole = new Pole();
        Bait bait = new Bait();

        bool on = true;
        while(on){
            int gold = 100;
            forecast.GenerateForecast(30, 50);
            pole.GenerateExpectedFish(forecast, fishSales);

            // Decide to skip or not
            Console.Write("Would you like to skip the day? Input \"y\" / \"n\"");
            string? res = Console.ReadLine();
            bool breaking = false;
            if (res != "y" && res != "n"){
                while(!breaking){
                    Console.Write("Not valid input. Please input y/n\nWould you like to skip the day? Input \"y\" / \"n\"");
                    res = Console.ReadLine();
                    if(res == "y" || res == "n"){
                        breaking = true;
                    }
                }
            }
            if(res == "y"){
                Console.Write("You Skipped! New Day!");
                continue;
            }

            // Decide which pole to buy
            Console.WriteLine("Which pole would you like to buy? Input \"s\" / \"m\" / \"l\"");
            Fish type = pole.RecommendPole();
            Console.WriteLine("Recommended pole is " + type.ToString() + " pole");
            res = Console.ReadLine();

            if (res != "s" && res != "m" && res != "l"){
                breaking = false;
                while(!breaking){
                    Console.Write("Not valid input. Please input s/m/l\nWhich pole would you like to buy? Input \"s\" / \"m\" / \"l\"");
                    res = Console.ReadLine();
                    if(res == "s" || res == "m" || res == "l"){
                        breaking = true;
                    }
                }
            }
            if(res == "s"){
                gold -= pole.BuyPole(Fish.S);
            }
            else if(res == "m"){
                gold -= pole.BuyPole(Fish.M);
            }
            else{
                gold -= pole.BuyPole(Fish.L);
            }
            Console.WriteLine("Gold left is " + gold);

            // Decide how much bait to buy
            Console.Write("This intelligent system will allocate ideal baits for your journey. Do you want to follow recommendation or do it manually? Input \"system\" or \"manual\"");
            res = Console.ReadLine();

            if (res != "system" && res != "manual"){
                breaking = false;
                while(!breaking){
                    Console.Write("Not valid input. Do you want to follow recommendation or do it manually? Input \"system\" or \"manual\"");
                    res = Console.ReadLine();
                    if(res == "system" || res == "manual"){
                        breaking = true;
                    }
                }
            }
            if(res == "system"){
                bait.BuyBait(forecast, pole.GetPoleType(), fishSales, gold);
            }
            else{
                bool valid = false;
                int[] baitCount = new int[3];
                while(!valid){

                    for(int i = 0; i < 3; i++){
                        Console.WriteLine(Enum.GetNames<Color>()[i] + " bait: ");
                        res = Console.ReadLine();
                        int value;
                        if (int.TryParse(res, out value)){
                            if (value <= 0){
                                breaking = false;
                                while(!breaking){
                                    Console.WriteLine("Not a valid number. Please input a number at least 0");
                                    res = Console.ReadLine();
                                    if(int.TryParse(res, out value) && value >= 0){
                                        breaking = true;
                                    }
                                }
                            }
                        }
                        baitCount[i] = value;
                    }
                    if (bait.CalculateTotalCost(baitCount) != gold){
                        Console.WriteLine("Doesn't add up to " + gold + "! try again");
                    }
                    else{
                        valid = true;
                    }
                }
                bait.SetRedBait(baitCount[0]);
                bait.SetBlueBait(baitCount[1]);
                bait.SetGreenBait(baitCount[2]);
            }
            gold = 0;

            // Fishing starts here

            bool done = false;
            int redFishCaught = 0;
            int blueFishCaught = 0;
            int greenFishCaught = 0;
            int revenue = 0;
            while(!done){
                Color chosenBait = bait.DetermineBait(forecast, type, fishSales);
                Console.WriteLine("Now fishing using " + chosenBait.ToString() + " bait");
                forecast.UpdateForecast(type, chosenBait);
                revenue += fishSales.CalculateActual((type, chosenBait));
                bait.UpdateBait(chosenBait);
                switch(chosenBait){
                    case Color.Red:
                        redFishCaught++;
                        break;
                    case Color.Blue:
                        blueFishCaught++;
                        break;
                    case Color.Green:
                        greenFishCaught++;
                        break;
                }
                Thread.Sleep(5000);

                Console.WriteLine("-------------------------------------------------------------------------------");
                Console.WriteLine("|  red bait  |  blue bait | green bait |  red fish  |  blue fish | green fish |");
                Console.WriteLine("|" + $"{bait.GetRedBait(), 12}" + "|" + $"{bait.GetBlueBait(), 12}" + "|" + $"{bait.GetGreenBait(), 12}" + "|" + $"{redFishCaught, 12}" + "|" + $"{blueFishCaught, 12}" + "|" + $"{greenFishCaught,12}" + "|");
                Console.WriteLine("| revenue: " + $"{revenue,-67}" +"|");
                Console.WriteLine("-----------------------------------------------------------------------------------");

                if(forecast.GetFishByCategory(type) == 0){
                    Console.WriteLine("No more fish");
                    Console.WriteLine("Final results: ");
                    
                    Console.WriteLine("-------------------------------------------------------------------------------");
                    Console.WriteLine("|  red bait  |  blue bait | green bait |  red fish  |  blue fish | green fish |");
                    Console.WriteLine("|" + $"{bait.GetRedBait(), 12}" + "|" + $"{bait.GetBlueBait(), 12}" + "|" + $"{bait.GetGreenBait(), 12}" + "|" + $"{redFishCaught, 12}" + "|" + $"{blueFishCaught, 12}" + "|" + $"{greenFishCaught,12}" + "|");
                    Console.WriteLine("| revenue: " + $"{revenue,-67}" +"|");
                    Console.WriteLine("-----------------------------------------------------------------------------------");
                    if(revenue > 100){
                        Console.WriteLine("You Win!");
                    } else{
                        Console.WriteLine("You Lose!");
                    };
                    done = true;
                }
            }
            Console.Write("Play Again? \"y\" / \"n\"");
            res = Console.ReadLine();
            breaking = false;
            if (res != "y" && res != "n"){
                while(!breaking){
                    Console.Write("Not valid input. Please input y/n\nPlay Again? \"y\" / \"n\"");
                    res = Console.ReadLine();
                    if(res == "y" || res == "n"){
                        breaking = true;
                    }
                }
            }
            if(res == "y"){
                continue;
            }
            else{
                on = false;
            }
        }
    }
}