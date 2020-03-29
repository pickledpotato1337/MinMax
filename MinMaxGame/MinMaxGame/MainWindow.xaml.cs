using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MinMaxGame
{
    public  class PlayerConfig
    {
        public static bool Protagonist { get; set; }
        public static List<int> inputs = new List<int>() { 6, 5, 4 };
        public static Tree gameTree = new Tree(21, inputs);


    }

    public class WinIndicator
    {
        private int _stepsReq;
        private int _childValue;
        private bool _isIntiated;
        public WinIndicator()
        {
            IsInitalted = false;
        }
        public int Stepsreq
        {
            get => _stepsReq;
            set
            {
                _stepsReq = value;

            }
        }
        public int ChildIndex
        {
            get => _childValue;
            set
            {
                _childValue = value;
            }
        }
        public bool IsInitalted
        {
            get => _isIntiated;
            set
            {
                _isIntiated = value;
            }
        }
    }

    public class Node
    {
        public List<Node> Children { get; set; }
        private Node _ancestor;
        private WinIndicator _nextIndicator;
        private int _cardValue;
        private int _level;
        private bool _victory;
        private bool _protagonist;
        private bool _gameOver;
        private bool _draw;
        private int _currentTally;

        public Node()
        {
            Victory = false;
            CardValue = 0;
            GameOver = false;
            Draw = false;
            Children = new List<Node>();
            NextIndicator = new WinIndicator();

        }
        public Node Ancestor
        {
            get => _ancestor;
            set
            {
                _ancestor = value;
            }
        }
        public WinIndicator NextIndicator
        {
            get => _nextIndicator;
            set
            {
                _nextIndicator = value;
            }
        }


        public int CardValue
        {
            get => _cardValue;
            set
            {
                _cardValue = value;
            }
        }

        public int Level
        {
            get => _level;
            set
            {
                _level = value;
            }
        }
        public bool Victory
        {
            get => _victory;
            set
            {
                _victory = value;
            }
        }
        public bool Protagonist
        {
            get => _protagonist;
            set
            {
                _protagonist = value;
            }
        }

        public bool GameOver
        {
            get => _gameOver;
            set
            {
                _gameOver = value;
            }
        }
        public bool Draw
        {
            get => _draw;
            set
            {
                _draw = value;
            }
        }
        public int Tally
        {

            get => _currentTally;
            set
            {
                _currentTally = value;
            }
        }


    }

    public class Tree
    {
        private bool player1; //player 1 is the protagonist, default true
        private int gameClock;
        private int totalTally;
        private Node Root;
        private List<int> cardValues;
        private List<Node> victories { get; set; }
        private List<Node> draws { get; set; }
        public string diagram;
        public Node walker;


        public Tree(int maxTally, List<int> inputVariables)
        {
            player1 = PlayerConfig.Protagonist;
            totalTally = maxTally;
            Root = new Node();
            cardValues = inputVariables;
            //cardValues.Sort();
            //cardValues.Reverse();
            victories = new List<Node>();
            draws = new List<Node>();
            gameClock = 0;
            walker = new Node();

        }

        // create tree, fill it, push victories on the list, find the ones with lowest level. 
        public void Init()
        {
            SwitchPlayer();
            Root.Protagonist = player1;
            var movingNode = new Node();
            
            movingNode = Root;
           
            GrowTree(movingNode);
            
            RetraceVictories();
            
            RetraceDraws();
            walker = Root;
           
        }

        public void GrowTree(Node currentNode)
        {
            int statusCheck = CheckVictory(currentNode);
            switch (statusCheck)
            {
                case 0:
                    gameClock++;
                    Construct(currentNode);
                    
                    foreach (Node element in currentNode.Children)
                    {
                        GrowTree(element);
                       
                    }
                    break;
                case 1:
                    currentNode.Victory = true;
                    currentNode.GameOver = true;
                    victories.Add(currentNode);
                    break;
                case 2:
                    currentNode.GameOver = true;
                    break;
                case 3:

                    currentNode.GameOver = true;
                    currentNode.Draw = true;
                    draws.Add(currentNode);
                    break;
            }
        }

        public void Construct(Node entryPoint)
        {
            foreach (int values in cardValues)
            {
                var traverse = new Node();
                traverse.Tally = entryPoint.Tally + values;
                entryPoint.Children.Add(traverse);
                traverse.Ancestor = entryPoint;
                traverse.CardValue = values;
                traverse.Level = gameClock;
                if (player1 == true)
                {
                    if (gameClock % 2 == 0)
                        traverse.Protagonist = true;
                    else
                        traverse.Protagonist = false;
                }
                else
                {
                    if (gameClock % 2 == 0)
                        traverse.Protagonist = false;
                    else
                        traverse.Protagonist = true;
                }
            }

        }

        public int CheckVictory(Node node)
        {

            if (node.Protagonist == false && node.Tally > totalTally)
            {
                //protagonist wins
                return 1;
            }
            if (node.Protagonist == true && node.Tally > totalTally)
            {
                //antagonist wins
                return 2;
            }
            if (node.Tally == totalTally)
            {
                //draw
                return 3;
            }
            //game goes on

            return 0;
        }

        public String CreateDiagram(Tree gameTree)
        {

            return null;
        }

        public void SwitchPlayer()
        {
            player1 = PlayerConfig.Protagonist;
        }

        public void RetraceVictories()
        {
            foreach (Node element in victories)
            {
                // this shit here is a bit trickier
                // i need to go from every victory up, and change indicators
                //every node needs indicator pointing only towards the lowest level
                Node parent = new Node();
                Node currentNode = new Node();
                currentNode = element;
                while (currentNode.Tally > 0)
                {

                    

                    parent = currentNode.Ancestor;
                    //may need alternative solution
                    if (parent.NextIndicator.IsInitalted == false || parent.NextIndicator.Stepsreq > element.Level)
                    {
                        
                        parent.NextIndicator.IsInitalted = true;
                        parent.NextIndicator.ChildIndex = parent.Children.IndexOf(element);
                        parent.NextIndicator.Stepsreq = element.Level;
                    }

                    currentNode = parent;

                }
            }
        }

        public void RetraceDraws()
        {
            foreach (Node element in draws)
            {
                // this shit here is a bit trickier
                // i need to go from every victory up, and change indicators
                //every node needs indicator pointing only towards the lowest level
                Node parent = new Node();
                Node currentNode = new Node();
                currentNode = element;
                while (currentNode.Tally > 0)
                {
                    parent = currentNode.Ancestor;
                    //may need alternative solution
                    if (parent.NextIndicator.IsInitalted == false)
                    {
                        parent.NextIndicator.IsInitalted = true;
                        parent.NextIndicator.ChildIndex = parent.Children.IndexOf(element);
                        parent.NextIndicator.Stepsreq = element.Level;
                    }

                    currentNode = parent;
                }

            }

        }

        public void MoveWalker(int childIndex)
        {
            walker = walker.Children[childIndex];
        }


        public string LevelOrder()
        {
            string lineBreak = Environment.NewLine;
            string res=" ";
            int levels = GetDepth();
            for(int depth=0; depth>levels; depth++)
            {
                res +=" "+ PrintLevel(Root, depth) +lineBreak;
            }

            return res;

        }

        public int GetDepth() {
            int depth = victories[0].Level;
            foreach (Node win in victories) {

                if (win.Level > depth)
                    depth = win.Level;

            }
            return depth;
                    }


        public string PrintLevel(Node traverse, int level)
        {
            string res = " ";
            if(traverse.Level == level)
            {
                
                res += " "+traverse.Tally.ToString() + " protagonist= " + traverse.Protagonist.ToString()+ " ";
                if (traverse.Victory == true)
                    res += " Victory ";
                if (traverse.Draw == true)
                    res += " Draw";
            }
            
            if (traverse.Level < level)
            {
                foreach(Node child in traverse.Children)
                {
                    PrintLevel(child, level);
                }
            }
            if (traverse == null)
                return res;
            return res;
        }

    }

        public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            


        }

        private void NgButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentTallyBox.Clear();
            TurnDisplayBox.Clear();
            ProtagonistCheatBox.Clear();
            Window1 PlayerSelectWindow = new Window1();
            PlayerSelectWindow.Show();
            CurrentTallyBox.Text = "0";
            TurnDisplayBox.Text = "player 1";
            

        }

        private void Add4Button_Click(object sender, RoutedEventArgs e)
        {
            CurrentTallyBox.Clear();
            TurnDisplayBox.Clear();
            ProtagonistCheatBox.Clear();
            PlayerConfig.gameTree.MoveWalker(2);
           
            CurrentTallyBox.Text = PlayerConfig.gameTree.walker.Tally.ToString();
            if (PlayerConfig.gameTree.walker.Protagonist == true) {
                int index = PlayerConfig.gameTree.walker.NextIndicator.ChildIndex;
                ProtagonistCheatBox.Text = PlayerConfig.inputs[index].ToString();

            }
            if (PlayerConfig.gameTree.walker.Level % 2 == 0)
                TurnDisplayBox.Text = "player1";
            if (PlayerConfig.gameTree.walker.Level % 2 == 1)
                TurnDisplayBox.Text = "player2";
            if (PlayerConfig.gameTree.walker.Victory == true)
                TurnDisplayBox.Text = "player1 wins";

            else if (PlayerConfig.gameTree.walker.Draw == true)
                TurnDisplayBox.Text = "draw";

            else if (PlayerConfig.gameTree.walker.GameOver == true)
                TurnDisplayBox.Text = "player2 wins";
        }

        private void Add5Btn_Click(object sender, RoutedEventArgs e)
        {
            CurrentTallyBox.Clear();
            TurnDisplayBox.Clear();
            ProtagonistCheatBox.Clear();
            PlayerConfig.gameTree.MoveWalker(1);
            CurrentTallyBox.Text = PlayerConfig.gameTree.walker.Tally.ToString();
            if (PlayerConfig.gameTree.walker.Protagonist == true)
            {
                int index = Math.Abs(PlayerConfig.gameTree.walker.NextIndicator.ChildIndex);
                ProtagonistCheatBox.Text = PlayerConfig.inputs[index].ToString();

            }
            if (PlayerConfig.gameTree.walker.Level % 2 == 0)
                TurnDisplayBox.Text = "player1";
            if (PlayerConfig.gameTree.walker.Level % 2 == 1)
                TurnDisplayBox.Text = "player2";
            if (PlayerConfig.gameTree.walker.Victory == true)
                TurnDisplayBox.Text = "player1 wins";

            else if (PlayerConfig.gameTree.walker.Draw == true)
                TurnDisplayBox.Text = "draw";

            else if (PlayerConfig.gameTree.walker.GameOver == true)
                TurnDisplayBox.Text = "player2 wins";


        }

        private void Add6Btn_Click(object sender, RoutedEventArgs e)
        {
            CurrentTallyBox.Clear();
            TurnDisplayBox.Clear();
            ProtagonistCheatBox.Clear();
            PlayerConfig.gameTree.MoveWalker(0);
            CurrentTallyBox.Text = PlayerConfig.gameTree.walker.Tally.ToString();
            if (PlayerConfig.gameTree.walker.Protagonist == true)
            {
                int index = PlayerConfig.gameTree.walker.NextIndicator.ChildIndex;
                ProtagonistCheatBox.Text = PlayerConfig.inputs[index].ToString();

            }
            if (PlayerConfig.gameTree.walker.Level % 2 == 0)
                TurnDisplayBox.Text = "player1";
            if (PlayerConfig.gameTree.walker.Level % 2 == 1)
                TurnDisplayBox.Text = "player2";
            if (PlayerConfig.gameTree.walker.Victory == true)
                TurnDisplayBox.Text = "player1 wins";

            else if (PlayerConfig.gameTree.walker.Draw == true)
                TurnDisplayBox.Text = "draw";

            else if (PlayerConfig.gameTree.walker.GameOver == true)
                TurnDisplayBox.Text = "player2 wins";

        }

        private void PrintTreeBtn_Click(object sender, RoutedEventArgs e)
        {
            Window2 printWindow = new Window2();
            printWindow.Show(); 
        }
    }
}
