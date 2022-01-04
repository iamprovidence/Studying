using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace ConsoleParking
{
    class Menu : IDisposable
    {
        // FIELDS
        private System.Net.Http.HttpClient client;

        bool exit;

        // CONSTRUCTORS
        public Menu()
        {
            client = new System.Net.Http.HttpClient();
        }
        public void Dispose()
        {
            client.Dispose();
        }

        // METHODS
        private void WriteLine(string text, ConsoleColor consoleColor)
        {
            Console.ForegroundColor = consoleColor;
            Console.WriteLine(text);
            Console.ResetColor();
        }
        public void Run()
        {
            while (!exit)
            {
                ShowMenuItems();
                int userChoice = GetInput();

                try
                {
                    Perform(userChoice);
                }
                catch (AggregateException ex)
                {
                    WriteLine(ex.Message, ConsoleColor.DarkYellow);
                    WriteLine("Server may not be running", ConsoleColor.DarkYellow);
                    Console.ReadLine();
                }
            }
        }

        private void Perform(int userChoice)
        {
            switch (userChoice)
            {
                case 0: GetParkingBalance();                break;
                case 1: GetMoneyEarnedInLastMinute();       break;
                case 2: GetAmountOfFreeAndBusyPlaces();     break;
                case 3: GetLastTransaction();               break;
                case 4: GetAllTransaction();                break;
                case 5: GetVehicleList();                   break;
                case 6: AddVehicle();                       break;
                case 7: RemoveVehicle();                    break;
                case 8: RefillVehicleBalance();             break;
                case 9: Exit();                             break;

                default: Console.WriteLine("Wrong choice"); break;
            }

            Console.WriteLine("Press enter");
            Console.ReadLine();
        }


        private int GetInput()
        {
            // get user choice
            int userChoice;
            do
            {
                Console.WriteLine("Your input:"); 
            } while (!int.TryParse(Console.ReadLine(), out userChoice)) ;

            return userChoice;
        }

        private void ShowMenuItems()
        {
            Console.Clear();

            Console.WriteLine("Choose option");

            Console.WriteLine();

            Console.WriteLine("0 - Get parking balance");
            Console.WriteLine("1 - The amount of money earned in the last minute");
            Console.WriteLine("2 - Find out the number of free/busy places in the parking lot");
            Console.WriteLine("3 - Show all parking Transactions in the last minute");
            Console.WriteLine("4 - Print the entire Transaction history");
            Console.WriteLine("5 - Display the list of all Vehicles");
            Console.WriteLine("6 - Park the Vehicle");
            Console.WriteLine("7 - Remove the vehicle");
            Console.WriteLine("8 - Refill the balance of the Vehicle");            
            WriteLine("9 - Exit", ConsoleColor.Red);

            Console.WriteLine();
        }

        // ACTIONS
        private void GetParkingBalance()
        {
            Console.WriteLine("Parking balance");

            string balance = client.GetStringAsync(@"https://localhost:5001/api/parking/balance").Result;
            decimal parkingBalance = decimal.Parse(balance, System.Globalization.CultureInfo.InvariantCulture);
            WriteLine(parkingBalance.ToString(), parkingBalance > 0 ? ConsoleColor.Green : ConsoleColor.Red);
        }
        
        private void GetMoneyEarnedInLastMinute()
        {
            Console.WriteLine("Money earned in last minute");
            Console.WriteLine(decimal.Parse(client.GetStringAsync(@"https://localhost:5001/api/parking/earned_money").Result, System.Globalization.CultureInfo.InvariantCulture));
        }
        
        private void GetAmountOfFreeAndBusyPlaces()
        {
            Console.WriteLine($"Total space = {int.Parse(client.GetStringAsync(@"https://localhost:5001/api/parking/capacity").Result)}");
            Console.WriteLine($"Free space = {int.Parse(client.GetStringAsync(@"https://localhost:5001/api/parking/free_spaces").Result)}");
            Console.WriteLine($"Busy space = {int.Parse(client.GetStringAsync(@"https://localhost:5001/api/parking/busy_spaces").Result)}");
        }
                              
        private void GetLastTransaction()
        {
            Console.WriteLine("Last transactions");
            foreach (ParkingSystem.Models.Transaction transaction in JsonConvert.DeserializeObject<List<ParkingSystem.Models.Transaction>>
                                                                (client.GetStringAsync(@"https://localhost:5001/api/transaction/last").Result))
            {
                Console.WriteLine(transaction.ToString());
            }
        }
        
        private void GetAllTransaction()
        {
            Console.WriteLine("All transactions");
            IEnumerable<string> rawTransaction = JsonConvert.DeserializeObject<IEnumerable<string>>
                                (client.GetStringAsync(@"https://localhost:5001/api/transaction/all").Result);

            if (rawTransaction != null)
            {
                foreach (string transactionRecords in rawTransaction)
                {
                    Console.WriteLine(transactionRecords);
                }
            }
            else
            {
                Console.WriteLine("Records file does not exist");
            }

        }        
        
        private void GetVehicleList()
        {
            Console.WriteLine("All vehicles");

            foreach (ParkingSystem.Vehicle.VehicleBase vehicle in JsonConvert.DeserializeObject<List<ParkingSystem.Vehicle.VehicleBase>>
                                                    (client.GetStringAsync(@"https://localhost:5001/api/vehicle/").Result))
            {
                Console.WriteLine(vehicle.ToString());
            }
        }
        
        private void AddVehicle()
        {
            // show vehicle type
            Console.WriteLine("Enter vehicle type");
            Console.WriteLine();

            Dictionary<int, string> vehicleTypes =
                JsonConvert.DeserializeObject<Dictionary<int, string>>(
                    client.GetStringAsync(@"https://localhost:5001/api/vehicle/types").Result);
            ShowVehicleTypes(vehicleTypes);
            

            // get user input
            int vehicleType = -1;
            do
            {
                Console.WriteLine("Write vehicle type:");
            } while (!int.TryParse(Console.ReadLine(), out vehicleType) || !vehicleTypes.ContainsKey(vehicleType));

            // create vehicle
            bool isAdded = client.PutAsync($@"https://localhost:5001/api/vehicle/{vehicleType}", new System.Net.Http.StringContent(String.Empty))
                            .Result.IsSuccessStatusCode;

            if (isAdded) WriteLine("New vehicle added", ConsoleColor.Green);            
            else         WriteLine("Can not add vehicle", ConsoleColor.Red);
            

        }
        private void ShowVehicleTypes(Dictionary<int, string> vehicleTypes)
        {
            foreach (KeyValuePair<int, string> vahicleType in vehicleTypes)
            {
                Console.WriteLine($"{vahicleType.Key} - {vahicleType.Value}");
            }
        }
        
        private void RemoveVehicle()
        {
            // get user input
            int vehicleId;
            do
            {
                Console.WriteLine("Enter vehicle id");
            } while (!int.TryParse(Console.ReadLine(), out vehicleId));

            // removing
            bool isRemoved = client.DeleteAsync($@"https://localhost:5001/api/vehicle/{vehicleId}").Result.IsSuccessStatusCode;

            if (isRemoved)  WriteLine("Vehicle is deleted", ConsoleColor.Green);
            else            WriteLine("Vehicle is not deleted", ConsoleColor.Red);
        }
        
        private void RefillVehicleBalance()
        {
            // get vehicle id
            int vehicleId;
            do
            {
                Console.WriteLine("Enter vehicle id:");
            } while (!int.TryParse(Console.ReadLine(), out vehicleId));
            
            // get money amount
            decimal moneyInput;
            do
            {
                Console.WriteLine("Enter money:");
            } while (!decimal.TryParse(Console.ReadLine(), out moneyInput));

            // refill balance
            bool isReffiled = client.PatchAsync($@"https://localhost:5001/api/vehicle/refill_balance/?vehicleId={vehicleId}&money={moneyInput}", new System.Net.Http.StringContent(String.Empty))
                                .Result.IsSuccessStatusCode;

            if (isReffiled) WriteLine("Balanced reffiled", ConsoleColor.Green);
            else            WriteLine("Balance does not reffilled", ConsoleColor.Red);
        }
        
        private void Exit()
        {
            exit = true;
        }

    }
}
