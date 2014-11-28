using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaPS_Task2
{
    public class User
    {
        //количество оставшихся попыток
        private int requestsLeft;
        public int RequestsLeft
        {
            get { return requestsLeft; }
        }

        //сколько хочет считать.
        private int amount;
        public int Amount
        {
            get { return amount; }
        }

        private bool ok;
        public bool OK
        {
            get { return ok; }
        }

        public User(int amount)
        {
            this.amount = amount;
            requestsLeft = 3;
            ok = false;
        }

        public void UseCashMachine(CashMachine machine)
        {
            //если в банкомате нет денег, то уходит
            if (machine.State == CashMachineState.NoMoney)
            {
                requestsLeft = 0;
                return;
            }

            if ((requestsLeft > 0) && (!ok))
            {
                ok = machine.TryGetMoney(amount);
                if (!ok)
                {
                    requestsLeft--;
                    if (amount % 50 != 0) //если операция не прошла из-за того, что сумма не делится на 50
                        amount = (amount / 50) * 50;
                    else //если в банкомате недостаточно денег
                    {
                        int tmp = amount;
                        amount = 0;
                        foreach (int k in machine.Money.Keys)
                        {
                            if (machine.Money[k] > 0)
                            {
                                amount += ((tmp / k > machine.Money[k]) ? machine.Money[k] : tmp / k) * k;
                                tmp -= amount;
                            }
                        }
                    }
                }
            }
        }
    }
}
