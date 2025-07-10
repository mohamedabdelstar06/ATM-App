using ATMApp.Domain.Entities;
using ATMApp.Domain.Interfaces;
using ATMApp.UI;

namespace ATMApp
{
    public class ATMApp : IUserLogin
    {

        private static List<UserAccount> userAccountList;
        private static UserAccount selectedAccount;



        public void InitializedData()
        {
            userAccountList = new List<UserAccount>
        {
            new UserAccount{Id =1, FullName="Abdelstar", AccountNumber=123456, CardNumber=321321, CardPin=123123, AccountBalance=50000.00m, IsLocked=false},
            new UserAccount{Id =2, FullName="Israa", AccountNumber=456789, CardNumber=654654, CardPin=456456, AccountBalance=40000.00m, IsLocked=false},
            new UserAccount{Id =3, FullName="Amira", AccountNumber=123555, CardNumber=987987, CardPin=789789, AccountBalance=30000.00m, IsLocked=true}
        };
        }
        public void CheckUserCardNumberAndPasswword()
        {
            bool isCorrectLogin = false;
            while (isCorrectLogin == false)
            {
                UserAccount inputAccount = AppScreen.UserLoginForm();
                AppScreen.LoginProgress();
                foreach (UserAccount account in userAccountList)
                {
                    selectedAccount =account;
                    if (inputAccount.CardNumber.Equals(selectedAccount.CardNumber))
                    {
                        selectedAccount.TotalLogin++;
                        if (inputAccount.CardPin.Equals(selectedAccount.CardPin))
                        {
                            selectedAccount = account;
                            if (selectedAccount.IsLocked || selectedAccount.TotalLogin > 3)
                            {
                                AppScreen.PrintLockScreen();
                            }
                            else
                            {
                                selectedAccount.TotalLogin =0;
                                isCorrectLogin =true;
                                break;
                            }
                        }
                    }
                    if (isCorrectLogin == false)
                    {
                        Utility.PrintMessage("\n Invalid Card number or PIN ", false);
                        selectedAccount.IsLocked = selectedAccount.TotalLogin ==3;
                        if (selectedAccount.IsLocked)
                        {
                            AppScreen.PrintLockScreen();
                        }
                    }
                    Console.Clear();
                }
            }

        }
        public void Welcome()
        {
            Console.WriteLine($"Welcome Back, {selectedAccount.FullName}");
        }

    }
}