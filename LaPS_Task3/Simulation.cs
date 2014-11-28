using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace LaPS_Task2
{
    public class SimulationEventArgs : EventArgs
    {
        private CashMachine cashMachine;
        public CashMachine CashMachine
        {
            get { return cashMachine; }
        }

        private Queue<User> users;
        public Queue<User> Users
        {
            get { return users; }
        }

        public SimulationEventArgs(CashMachine cashMachine, Queue<User> users)
        {
            this.cashMachine = cashMachine;
            this.users = users;
        }
    }

    public delegate void SimulationEventHandler(object sender, SimulationEventArgs e);

    public class Simulation
    {
        public CashMachine CashMachine { get; set; }
        public Queue<User> Users { get; set; }

        private Timer timer;
        private Random rnd = new Random();

        private Timer queueTimer;

        public event SimulationEventHandler OnSimulation = null;

        public Simulation(CashMachine cashMachine, int N, int M)
        {
            CashMachine = cashMachine;
            Users = new Queue<User>();
            timer = new Timer(cashMachine.Time);
            queueTimer = new Timer(1000);
            queueTimer.Elapsed += (sender, e) =>
            {
                if ((rnd.Next(1000) > 500) && (CashMachine.State == CashMachineState.Working) && (Users.Count <= 4))
                {
                    
                    Users.Enqueue(new User(rnd.Next(N, M)));
                }
                if (OnSimulation != null)
                    OnSimulation(this, new SimulationEventArgs(CashMachine, Users));
            };
            timer.Elapsed += (sender, e) =>
            {
                if (Users.Count > 0)
                {
                    User topUser = Users.Peek();

                    if (topUser.RequestsLeft > 0)
                    {
                        topUser.UseCashMachine(CashMachine);
                    }
                    if (topUser.OK || (topUser.RequestsLeft == 0))
                        Users.Dequeue();
                    if (CashMachine.State == CashMachineState.NoMoney)
                        Users.Clear();
                }

                if (OnSimulation != null)
                    OnSimulation(this, new SimulationEventArgs(CashMachine, Users));
            };
            queueTimer.Start();
            timer.Start();
        }
    }
}
