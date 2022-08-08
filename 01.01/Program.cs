using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01._01
{
    class Weapon
    {
        public int Damage;
        public int Bullets;

        public void Fire(Player player)
        {

            player.Health -= Damage;
            Bullets -= 1;

        }
    }

    class Player
    {
        public int Health;
    }

    class Bot
    {
        public Weapon Weapon;

        public void OnSeePlayer(Player player)
        {
            Weapon.Fire(player);
        }
    }
}
