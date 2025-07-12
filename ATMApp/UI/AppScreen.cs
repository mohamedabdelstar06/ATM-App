using ATMApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMApp.UI;
public  class AppScreen
{
    internal const string cur = " EGP ";
    internal static void Welcome()
    {
        //clear the console screen 
        Console.Clear();
        //set the title of the console window and foreground color
        Console.Title = "My ATM App";
        Console.WriteLine("Hello, World!");
        Console.ForegroundColor = ConsoleColor.White;

        // sets the welcome message 
        Console.WriteLine("\n\n--------------Welcome To My ATM App--------------\n\n");
        // prompt the  user to insert the ATM Card
        Console.WriteLine("Please insert your ATM card");
        Console.WriteLine("Note : Actual ATM machine will accept and validate " +
            "a physical ATM card , Read the card number and validate it");
        Utility.PressEnterToContinue();

    }
    internal static UserAccount UserLoginForm()
    {
        UserAccount tempUserAccount = new UserAccount();

        tempUserAccount.CardNumber =Validator.Convert<long>("your card number .");
        tempUserAccount.CardPin = Convert.ToInt32(Utility.GetSecretInput("Enter your Card PIN"));
        return tempUserAccount; 
    }
    internal static void LoginProgress()
    {
        Console.WriteLine("\nChecking card number and PIN...");
        Utility.PrintDotAnimation();

    }
    internal static void PrintLockScreen()
    {
        Console.Clear();
        Utility.PrintMessage("Your account is blocked . Please go to the nearest branch"+
            "to unlock your account. Thank you.  ", true);
        
        Environment.Exit(1);
    }
    internal static void WelcomeCustomer(string fullName)
    {
        Console.WriteLine($"Welcome Back, {fullName}");

        Utility.PressEnterToContinue();
    }
    internal static void DisplayAppMenu()
    {
        Console.Clear();
        Console.WriteLine("----------My ATM APP Menu---------");
        Console.WriteLine(":                                :");
        Console.WriteLine("1. Account Balance               :");
        Console.WriteLine("2. Cash Deposit                  :");
        Console.WriteLine("3. Widthdrawal                   :");
        Console.WriteLine("4. Transfer                      :");
        Console.WriteLine("5. Trunsaction                   :");
        Console.WriteLine("6. Logout                        :\n");
    }
    internal static void LogoutProgress()
    {
        Console.WriteLine("Thank you for using My ATM App.");
        Utility.PrintDotAnimation();
        Console.Clear();
    }
    internal static int SelectAmount()
    {
        Console.WriteLine("");
        Console.WriteLine(":1.{0}500      5.{0}10,000", cur);
        Console.WriteLine(":2.{0}1000     6.{0}15,000", cur);
        Console.WriteLine(":3.{0}2000     7.{0}20,000", cur);
        Console.WriteLine(":4.{0}5000     8.{0}40,000", cur);
        Console.WriteLine(":0.Other", cur);
        Console.WriteLine("");

        int selectedAccount = Validator.Convert<int>("option:");
        
        switch (selectedAccount)
        {
            case 1: return 500;
            case 2: return 1000;
            case 3: return 2000;
            case 4: return 5000;
            case 5: return 10000;
            case 6: return 15000;
            case 7: return 20000;
            case 8: return 40000;
            case 0: return 0;
            default:
                Utility.PrintMessage("Invalid Input. Try again.", false);
                return -1;
                break;
        }
    }
    internal InternalTransfer InternalTransferForm()
    {
        var internalTransfer = new InternalTransfer();
        internalTransfer.ReciepeintBankAccountNumber = Validator.Convert<long>("recipient account number");
        internalTransfer.TransferAmount = Validator.Convert<decimal>($"amount {cur}");
        internalTransfer.ReciepeintBankAccountName = Utility.GetUserInput("recipient name");
        return internalTransfer;
    }

}
