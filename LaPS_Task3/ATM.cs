using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaPS_Task2
{
    public enum CashMachineState
    {
        Working, NoMoney
    }

    public class CashMachine
    {
        private IDictionary<int, int> money;
        public IDictionary<int, int> Money
        {
            get { return money; }
        }

        private CashMachineState state;
        public CashMachineState State
        {
            get { return state; }
        }

        //время на одну операцию
        public int Time { get; set; }

        public CashMachine(int money50, int money100, int money500, int money1000, int money5000, int time)
        {
            this.money = new Dictionary<int, int>();
            money.Add(5000, money5000);
            money.Add(1000, money1000);
            money.Add(500, money500);
            money.Add(100, money100);
            money.Add(50, money50);
            ReloadSum();

            Time = time;
        }

        private int sum;
        private int maxV;
        private void ReloadSum()
        {
            sum = 0; maxV = 0;
            foreach (int k in money.Keys)
                sum += money[k] * k;
            if (sum == 0) state = CashMachineState.NoMoney;
            else state = CashMachineState.Working;

            foreach (int k in money.Keys)
                if ((money[k] != 0) && (maxV == 0)) maxV = k;
        }
        public int Sum
        {
            get { return sum; }
        }

        public int MaxValue
        {
            get { return maxV; }
        }

        //пытаемся взять деньги
        public bool TryGetMoney(int amount)
        {
            //если сумма не делится на 50, то операция отвергается
            if (amount % 50 != 0) return false;
            else
            {
                Dictionary<int, int> given = new Dictionary<int, int>();
                foreach (int k in money.Keys)
                {
                    if (amount > 0)
                    {
                        given[k] = amount / k;
                        if (given[k] > money[k])
                            given[k] = money[k];
                        amount -= given[k] * k;
                    }
                }

                //если можно выдать деньги, то выдаем их
                if (amount == 0)
                {
                    IDictionary<int, int> tmp = new Dictionary<int, int>();
                    foreach (int k in money.Keys)
                    {
                        if (given.ContainsKey(k))
                            tmp[k] = money[k] - given[k];
                        else tmp[k] = money[k];
                    }
                    money = tmp;
                    ReloadSum();
                    return true;
                }

                //в противном случае, отклоняем операцию
                return false;
            }
        }
    }
}
