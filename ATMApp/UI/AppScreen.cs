using ATMApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMApp.UI;
public static class AppScreen
{
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
        Console.WriteLine("\nChicking card number and PIN...");
        Utility.PrintDotAnimation();

    }
    internal static void PrintLockScreen()
    {
        Console.Clear();
        Utility.PrintMessage("Your account is blocked . Please go to the nearest branch"+
            "to unlock your account. Thank you.  ", true);
        Utility.PressEnterToContinue();
        Environment.Exit(1);
    }

}
