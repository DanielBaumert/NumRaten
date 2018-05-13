using System;
using System.Linq;
using System.Collections.Generic;

class NumRaten { 
	private static Random Rnd;
	public static void Main(string[] args){ 
		Rnd = new Random();
		Console.Clear();
		int input = 0;
		do{
			Console.WriteLine("Wilkommen zu NumRaten!");
			Console.WriteLine("Single Player\t[{0}]", (int) PlayMode.Single);
			Console.WriteLine("Multi Player\t[{0}]", (int) PlayMode.Multi);
			ReadLine("Bitte wählen Sie:", out input);
			switch(input) { 
				case (int) PlayMode.Single: 
					break;
				case (int) PlayMode.Multi:
					MultiPlay();
					break;
				default: 
					Console.Clear();
					Console.WriteLine("Bitte wählen Sie nur mögliche Optionen!!!");	
					input = (int) PlayMode.None;
					break;
			}
		}while(input == (int) PlayMode.None);
	}
	
	private static void MultiPlay() { 
		Console.Clear();
		Console.WriteLine("Wilkommen im Multi-Player");
		int playCount = 0;
		do{ 
			ReadLine("Spieler anzahl:", out playCount);
			if(playCount <= 1){
				Console.Clear();
				Console.WriteLine("Bitte wählen Sie mehr als 1 Spieler aus!!!");
			}
		}while(playCount <= 1);
		
		List<Player> player = new List<Player>();
		for(int i = 0; i < playCount; i += 1){ 
			string playerName = string.Empty;
			ReadLine($"Spielername {i+1}:", out playerName);
			Player p = new Player(playerName);
			player.Add(p);
		}
		int input = 0;
		int[] numberRange = new int[2];
		do { 
			Console.Clear();
			Console.WriteLine("In welchem Zahlenbreich möchten Sie Spielen?");
			ReadLine("Start:", out numberRange[0]);
			ReadLine("Ende:", out numberRange[1]);
			Console.WriteLine($"Der Zahlenbreich liegt zwischen {numberRange[0]} und {numberRange[1]}");
			ReadLine("Bestätigen (Ja[1]/Nein[0])", out input);
		}while(input == (int) ConfirmItem.No);
		
		Console.Clear();
		
		do{
			for(int i = 0; i < playCount; i += 1){
				Player mainer = player[i];
				do{
					Console.WriteLine($"Spieler {mainer.Name}, bitte wähle eine Zahl zwischen von {numberRange[0]} bis {numberRange[1]}");
					ReadLine("Wahl:", out input);
					if(!(numberRange[0] < input && input < numberRange[1])){
						Console.Clear();
						Console.WriteLine("Deine Zahl ist nicht zulässig, {0}", mainer.Name);
					}
				}while(!(numberRange[0] <= input && input <= numberRange[1]));
				int chooseNumber = input;

				Console.Clear();

				List<Player> otherPlayer = player.Where(x => x != mainer).ToList();	
				int selectPlayer = 0; 

				while(true){ 
					int number = 0; 
					Player x = otherPlayer[selectPlayer];
					Console.WriteLine("Sie sind am Zug, {0}", x.Name);
					ReadLine("Bitte raten Sie ein Zahl:", out number);
					
					if(number == chooseNumber){
						x.AddPoint();
						Console.WriteLine($"Du, {x.Name} hast richtig geraten und bekommst einen Punkt");
						Console.WriteLine("{0} hat nun {1} Punkte", x.Name, x.Points);
						break;
					} else { 
						if(number < chooseNumber){
							Console.WriteLine("Die Zahl {0}, ist zu klein!", number);
						} else {
							Console.WriteLine("Die Zahl {0}, ist zu groß!", number);
						}	
					}

					if(selectPlayer + 1 >= otherPlayer.Count){
						selectPlayer = 0;
					} else {  
						selectPlayer += 1;
					}
				}
			}
			Console.Clear();
			player.ForEach(p => Console.WriteLine($"{p.Name}\tPunkte:{p.Points}"));
			ReadLine("Möchen Sie weiter Spielen (Ja[1]/Nein[0]):", out input);
		}while(input != (int) ConfirmItem.No);
	}
	
	private class Player { 
		public string Name { get; set; }
		public int Points { get; private set; }		

		public Player(string name){
			Name = name;
			Points = 0;
		}
		public void AddPoint() 
			=> Points += 1;
		public void AddPoints(int count) 
			=> Points += count;
	}

	private static void ReadLine(string content,out string output){ 
		Console.Write(content);
		output = Console.ReadLine();
	}
	private static void ReadLine(string content, out int output){ 
		Console.Write(content);
		string input = Console.ReadLine();
		if(!int.TryParse(input, out output)){
			output = -1;
		}	
	}

	private enum PlayMode { 
		None = -1,
		Single = 1,
		Multi = 2
	}

	private enum ConfirmItem { 
		No = 0,
		Yes = 1
	}
}
