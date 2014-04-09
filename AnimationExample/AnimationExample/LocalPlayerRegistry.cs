using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ProjectSneakyGame
{
    public class LocalPlayerRegistry
    {
        static public int MAX_LOCAL_MACHINE_PLAYERS = 4;

        static private Player _player1;
        static private Player _player2;
        static private Player _player3;
        static private Player _player4;

        static public Player Player1 { get { return _player1; } }
        static public Player Player2 { get { return _player2; } }
        static public Player Player3 { get { return _player3; } }
        static public Player Player4 { get { return _player4; } }

        static public List<Player> ActivePlayerList
        {
            get
            {
                List<Player> amount = new List<Player>();
                if (_player1 != null)
                    amount.Add(_player1);
                if (_player2 != null)
                    amount.Add(_player2);
                if (_player3 != null)
                    amount.Add(_player3);
                if (_player4 != null)
                    amount.Add(_player4);
                return amount;
            }
        }

        static public List<Player> PlayerList
        {
            get
            {
                List<Player> amount = new List<Player>();
                amount.Add(_player1);
                amount.Add(_player2);
                amount.Add(_player3);
                amount.Add(_player4);
                return amount;
            }
        }

        static public void InitialisePlayer(PlayerIndex index)
        {
            switch (index)
            {
                case PlayerIndex.One:
                    _player1 = new Player(index);
                    break;

                case PlayerIndex.Two:
                    _player2 = new Player(index);
                    break;

                case PlayerIndex.Three:
                    _player3 = new Player(index);
                    break;

                case PlayerIndex.Four:
                    _player4 = new Player(index);
                    break;
            }
        }

        static public void InitialiseAllPlayers()
        {
            _player1 = new Player(PlayerIndex.One);
            _player2 = new Player(PlayerIndex.Two);
            _player3 = new Player(PlayerIndex.Three);
            _player4 = new Player(PlayerIndex.Four);
        }
    }
}
