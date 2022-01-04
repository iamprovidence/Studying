using System;

using ParkingSystem;
using static ParkingSystem.Core.Configurations;

namespace ConsoleParking
{
    class Menu
    {
        // FIELDS
        Parking parking;

        bool exit;

        // CONSTRUCTORS
        public Menu()
        {
            parking = Parking.GetInstance;
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
                Perform(userChoice);
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

            decimal parkingBalance = parking.Balance;
            WriteLine(parkingBalance.ToString(), parkingBalance > 0 ? ConsoleColor.Green : ConsoleColor.Red);
        }
        private void GetMoneyEarnedInLastMinute()
        {
            Console.WriteLine("Money earned in last minute");
            Console.WriteLine(parking.TransactionService.EarnedMoney());
        }
        private void GetAmountOfFreeAndBusyPlaces()
        {
            Console.WriteLine($"Total space = {parking.ParkingPlace.ParkingCapacity}");
            Console.WriteLine($"Free space = {parking.ParkingPlace.FreeSpacesAmount}");
            Console.WriteLine($"Busy space = {parking.ParkingPlace.BusySpacesAmount}");
        }
                              
        private void GetLastTransaction()
        {
            Console.WriteLine("Last transactions");
            foreach (ParkingSystem.Models.Transaction transaction in parking.TransactionService.Transactions)
            {
                Console.WriteLine(transaction.ToString());
            }
        }

        private void GetAllTransaction()
        {
            Console.WriteLine("All transactions");
            if (System.IO.File.Exists(TRANSACTION_LOG_FILE))
            {
                foreach (string transactionRecords in parking.TransactionService.GetRawTransactionFromFile(TRANSACTION_LOG_FILE))
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

            foreach (ParkingSystem.Vehicle.VehicleBase vehicle in parking.ParkingPlace.Vehicles)
            {
                Console.WriteLine(vehicle.ToString());
            }
        }

        private void AddVehicle()
        {
            // show vehicle type
            Console.WriteLine("Enter vehicle type");
            Console.WriteLine();
            ShowVehicleType();

            // get user input
            int vehicleType = -1;
            do
            {
                Console.WriteLine("Write vehicle type:");
            } while (!int.TryParse(Console.ReadLine(), out vehicleType) || !IsInVehicleRange(vehicleType));

            // create vehicle
            ParkingSystem.Vehicle.VehicleBase vehicle = CreateVehicle(vehicleType);
            bool isAdded = parking.ParkingPlace.AddCar(vehicle);

            if (isAdded)
            {
                WriteLine("New vehicle added", ConsoleColor.Green);
                Console.WriteLine(vehicle.ToString());
            }
            else
            {
                WriteLine("Can not add vehicle", ConsoleColor.Red);
            }

        }
        private void ShowVehicleType()
        {
            Console.WriteLine("0 - Bus");
            Console.WriteLine("1 - Motorcycle");
            Console.WriteLine("2 - Passenger car");
            Console.WriteLine("3 - Truck");
        }
        private ParkingSystem.Vehicle.VehicleBase CreateVehicle(int vehicleType)
        {
            switch (vehicleType)
            {
                case 0: return new ParkingSystem.Vehicle.Bus();
                case 1: return new ParkingSystem.Vehicle.Motorcycle();
                case 2: return new ParkingSystem.Vehicle.PassengerCar();
                case 3: return new ParkingSystem.Vehicle.Truck();

                default: throw new ArgumentException("Wrong vehicle type");                    
            }
        }
        private bool IsInVehicleRange(int vehicleType)
        {
            return vehicleType >= 0 && vehicleType < 4;
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
            bool isRemoved = parking.ParkingPlace.DeleteCar(vehicleId);

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

            // get vehicle
            ParkingSystem.Vehicle.VehicleBase vehicle = parking.ParkingPlace.GetVehicleById(vehicleId);
            if (vehicle == null)
            {
                WriteLine("No vehicle found", ConsoleColor.Red);
                return;
            }

            // get money amount
            decimal moneyInput;
            do
            {
                Console.WriteLine("Enter money:");
            } while (!decimal.TryParse(Console.ReadLine(), out moneyInput));

            // refill balance
            vehicle.Balance += moneyInput;
            WriteLine("Balanced reffiled", ConsoleColor.Green);
        }

        private void Exit()
        {
            exit = true;
        }

    }
}
