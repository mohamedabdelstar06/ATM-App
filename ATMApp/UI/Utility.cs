﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ATMApp.UI;

public static class Utility
{

    private static readonly CultureInfo culture;
    private static long tranId;
    static Utility()
    {
        culture = new CultureInfo("ar-EG").Clone() as CultureInfo;
        culture.NumberFormat.CurrencySymbol = "EGP";
        culture.NumberFormat.CurrencyDecimalSeparator = ".";
        culture.NumberFormat.CurrencyGroupSeparator = ",";
    }

    public static long getTransactionId()
    {
        return ++tranId;
    }

    public static string GetSecretInput(string prompt)
    {
        bool isPrompt = true;
        string asterics = "";
        StringBuilder input = new StringBuilder();

        while (true)
        {
            if (isPrompt)
                Console.WriteLine(prompt);
            isPrompt =false;
            ConsoleKeyInfo inputKey = Console.ReadKey(true);

            if (inputKey.Key == ConsoleKey.Enter)
            {
                if (input.Length == 6)
                {
                    break;
                }
                else
                {
                    PrintMessage("Please Enter 6 digits.", false);
                    input.Clear();
                    isPrompt =true;
                    continue;
                }
            }
            if (inputKey.Key == ConsoleKey.Backspace && input.Length > 0)
            {
                input.Remove(input.Length-1, 1);
            }
            else if (inputKey.Key != ConsoleKey.Backspace)
            {
                input.Append(inputKey.KeyChar);
                Console.Write(asterics + "*");
            }
        }
        return input.ToString();
    }




    public static void PrintMessage(string msg, bool success = true)
    {
        if (success)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }
        Console.WriteLine(msg);
        Console.ForegroundColor =ConsoleColor.White;
        PressEnterToContinue();
    }
    public static string GetUserInput(string prompt)
    {
        Console.WriteLine($"Enter {prompt}");
        return Console.ReadLine();
    }

    public static void PrintDotAnimation(int timer = 10)
    {
        
        for (int i = 0; i < timer; i++)
        {
            Console.Write(".");
            Thread.Sleep(200);
        }
        Console.Clear();
    }
    public static void PressEnterToContinue()
    {
        Console.WriteLine("\n\nPress Enter To Continue\n\n");
        Console.ReadLine();

    }
    public static string FormatAmount(decimal amt)
    {
        return amt.ToString("C2", culture);
    }

}
