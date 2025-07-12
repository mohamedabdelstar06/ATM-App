using ATMApp.Domain.Entities;
using ATMApp.Domain.Enums;
using ATMApp.Domain.Interfaces;
using ATMApp.UI;

namespace ATMApp
{
    public class ATMApp : IUserLogin , IUserAccountAction ,ITransaction
    {
        
        private List<UserAccount> userAccountList;
        private UserAccount ? selectedAccount;
        private List<Transaction> _listOfTransaction;
        private const decimal minimumKeptAmount = 500;
        private readonly AppScreen screen;

        public ATMApp()
        {
            screen =new AppScreen();
        }


        public void Run()
        {
            AppScreen.Welcome();
            InitializedData();
            CheckUserCardNumberAndPasswword();
            AppScreen.WelcomeCustomer(selectedAccount.FullName);
            AppScreen.DisplayAppMenu();
            ProcessMenuOption();
        }

        public void InitializedData()
        {
            userAccountList = new List<UserAccount>
        {
            new UserAccount{Id =1, FullName="Abdelstar", AccountNumber=123456, CardNumber=321321, CardPin=123123, AccountBalance=50000.00m, IsLocked=false},
            new UserAccount{Id =2, FullName="Israa", AccountNumber=456789, CardNumber=654654, CardPin=456456, AccountBalance=40000.00m, IsLocked=false},
            new UserAccount{Id =3, FullName="Amira", AccountNumber=123555, CardNumber=987987, CardPin=789789, AccountBalance=30000.00m, IsLocked=true}
        };
            _listOfTransaction = new List<Transaction>();
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
        private void ProcessMenuOption()
        {
            switch (Validator.Convert<int>("An option : "))
            {
                case (int)AppMenu.CheckBalance:
                    CheckBalance();
                    break;
                case (int)AppMenu.PlaceDeposit:
                    PlaceDeposit();
                    break;
                case (int)AppMenu.MakeWidthDrawal:
                    MakeWidthDrawal();
                    break;
                case (int)AppMenu.InternalTransfer:
                    var internalTransfer = screen.InternalTransferForm();
                    ProcessInternalTransfer(internalTransfer);
                    break;
                case (int)AppMenu.ViewTransaction:
                    Console.WriteLine("Viewing Transaction.... ");
                    break;
                case (int)AppMenu.Logout:
                    AppScreen.LogoutProgress();
                    Utility.PrintMessage("You have successfully logged out. Please collect"+
                        "your ATM card"); 
                    break;
                default:
                    Utility.PrintMessage("Invalid option ",false);
                    break;
            }
        }

        public void CheckBalance()
        {
            Utility.PrintMessage($"Your Account Palance is {Utility.FormatAmount(selectedAccount.AccountBalance)}");   
        }

        public void PlaceDeposit()
        {
            Console.WriteLine("\nOnly multiples of 500 and 1000 EGP allowed. \n");

            int transaction_amt = 0;

            while (true)
            {
                transaction_amt = Validator.Convert<int>($"amount {AppScreen.cur}");

                if (transaction_amt <= 0)
                {
                    Utility.PrintMessage("Amount needs to be greater than Zero. Try Again", false);
                    continue;
                }

                if (transaction_amt % 500 != 0)
                {
                    Utility.PrintMessage("Enter deposit amount in multiples of 500 or 1000. Try again", false);
                    continue;
                }

                break; // الرقم صحيح، نخرج من الـ loop
            }

            // simulate counting
            Console.WriteLine("\nChecking and Counting Bank notes.");
            Utility.PrintDotAnimation();
            Console.WriteLine("");

            if (PreviewBankNotesCount(transaction_amt) == false)
            {
                Utility.PrintMessage($"You have cancelled your action.", false);
                return;
            }

            // bind transaction details to transaction object
            InsertTransaction(selectedAccount.Id, TransactionType.Deposite, transaction_amt, "");

            // Update account balance 
            selectedAccount.AccountBalance += transaction_amt;

            // print success message
            Utility.PrintMessage($"Your deposit of {Utility.FormatAmount(transaction_amt)} was successful", true);
        }

        public void MakeWidthDrawal()
        {
            var transaction_amt = 0;
            int selectedAmount = AppScreen.SelectAmount();
            if(selectedAmount == -1)
            {
                selectedAmount = AppScreen.SelectAmount();

            }else if (selectedAmount!=0)
            {
                transaction_amt = selectedAmount;
            }
            else
            {
                transaction_amt = Validator.Convert<int>($"amount {AppScreen.cur}");

            }

            //input validation 
            if (transaction_amt <=0)
            {
                Utility.PrintMessage("Amount needs to be greater than zero. Try again", false);
                return ;
            }
            if(transaction_amt % 500 != 0)
            {
                Utility.PrintMessage("You can only withdraw amount in multiple of 500 or 1000 EGP. Try Again", false);
                return;
            }
            // Business logic validation 
            if (transaction_amt > selectedAccount.AccountBalance)
            {
                Utility.PrintMessage($"Withdrawal failed. Your balance is too low to withdraw"+
                    $"{Utility.FormatAmount(transaction_amt)}",false);
                return;
            }
            if((selectedAccount.AccountBalance - transaction_amt<minimumKeptAmount))
            {
                Utility.PrintMessage($"Withdrawal failed. Your account needs to have "+
                    $"minimum {Utility.FormatAmount(minimumKeptAmount)}",false);
                return;
            }
            //Bind widthdrawal details to transaction object
            InsertTransaction(selectedAccount.Id, TransactionType.Withdrawal, transaction_amt, "");
            //Update account balance 
            selectedAccount.AccountBalance -= transaction_amt;
            // Success message 
            Utility.PrintMessage("You have successfully withdrawn" + $"{Utility.FormatAmount(transaction_amt)}.", true);

        }
        private bool PreviewBankNotesCount(int amount)
        {
            int thousandNotesCount = amount / 1000;
            int fiveHundredNotes = (amount % 1000) / 500;
            Console.WriteLine("\nSummary");
            Console.WriteLine("-------");
            Console.WriteLine($"{AppScreen.cur} 1000 X {thousandNotesCount} = {1000 * thousandNotesCount  }");
            Console.WriteLine($"{AppScreen.cur} 500 X {fiveHundredNotes} = {500 * fiveHundredNotes} ");
            Console.WriteLine($"Total amount : {Utility.FormatAmount(amount)}\n\n");
            int opt = Validator.Convert<int>(" 1 to confirm");
            return opt.Equals(1);
        }

        public void InsertTransaction(long _userBankAccountId, TransactionType _transactionType, decimal _transactionAmount, string _desc)
        {
            //create a new transaction object
            var transaction = new Transaction()
            {
                TransactionId = Utility.getTransactionId(),
                UserBankAccountId  = _userBankAccountId,
                TransactionDate =DateTime.Now,
                TransactionType = _transactionType,
                TransactionAmount = _transactionAmount,
                Description = _desc
            };
            // Add Transaction object to the list
            _listOfTransaction.Add(transaction);
        }

        public void ViewTransaction()
        {
            throw new NotImplementedException();
        }

        private void ProcessInternalTransfer(InternalTransfer internalTransfer)
        {
            if (internalTransfer.TransferAmount <= 0)
            {
                Utility.PrintMessage("Amount needs to be more than zero. Try again.", false);
                return;
            }
            if (internalTransfer.TransferAmount > selectedAccount.AccountBalance)
            {
                Utility.PrintMessage($"Transfer failed. You do not have enough balance" +
                    $"to transfer {Utility.FormatAmount(internalTransfer.TransferAmount)}", false);
                return;
            }
            //check the minimum kept amount 
            if ((selectedAccount.AccountBalance - minimumKeptAmount) < minimumKeptAmount)
            {
                Utility.PrintMessage($"Transfer fail. Your account needs to have minimum " +
                    $"{Utility.FormatAmount(minimumKeptAmount)}", false);
                return;
            }
            //Check reciever account number is valid 
            var selectedBankAccountReciever = (
                from userAcc in userAccountList
                where userAcc.AccountNumber ==internalTransfer.ReciepeintBankAccountNumber
                select userAcc).FirstOrDefault();

            if (selectedBankAccountReciever == null)
            {
                Utility.PrintMessage("Transfer failed . Recieber bank account number is invalid.", false);
                return;
            }

            //Check reciever account name is valid 
            if (selectedBankAccountReciever.FullName != internalTransfer.ReciepeintBankAccountName)
            {
                Utility.PrintMessage("Transfer Failed. Recipient bank account name is not match", false);
                return;
            }
            //add transaction to transaction record sender 
            InsertTransaction(selectedAccount.Id, TransactionType.Transfer, -internalTransfer.TransferAmount, $"Transfered" +
                $"to {selectedBankAccountReciever.AccountNumber} ({selectedBankAccountReciever.FullName})");

            //Update sender account balance 
            selectedAccount.AccountBalance -= internalTransfer.TransferAmount;

            //Add transaction record reciever
            InsertTransaction(selectedBankAccountReciever.Id, TransactionType.Transfer, internalTransfer.TransferAmount, $"Transfered from" +
                    $"{selectedAccount.AccountNumber}({selectedAccount.FullName})");

            //Update reciever account balance
            selectedBankAccountReciever.AccountBalance += internalTransfer.TransferAmount;

            //Print success message
            Utility.PrintMessage($"You have successfully transfer" +
                $"{Utility.FormatAmount(internalTransfer.TransferAmount)} to " +
                $"{internalTransfer.ReciepeintBankAccountName}", true);
        }
    }
}



