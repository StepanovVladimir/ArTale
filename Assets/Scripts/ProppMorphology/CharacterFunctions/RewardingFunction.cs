using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.ProppMorphology.CharacterFunctions
{
    class RewardingFunction : ICharacterFunction
    {
        public RewardingFunction(TaleKind taleKind, string place, Characters characters, bool rewardIsMarriage)
        {
            SceneIndex = 12;
            Place = place;
            ActingCharacter = characters.Parent;
            AdditionalCharacter1 = characters.Hero;
            if (characters.Hero.Equals("Иван"))
            {
                StorylineString = "Царь женит Ивана на царевне";
            }
            else if (characters.Hero.Equals("Фролка"))
            {
                StorylineString = "Царь женит Фролку на царевне";
            }
            else if (characters.Hero.Equals("Зорька"))
            {
                StorylineString = "Царь женит Зорьку на царевне";
            }
            else
            {
                StorylineString = "Царь женит Никиту Кожемяку на царевне";
            }
            StorylineString = "Царь женит Ивана на царевне";
            if (rewardIsMarriage)
            {
                AdditionalCharacter2 = characters.Victim;
            }
            else
            {
                AdditionalCharacter2 = "Деньги из казны";
            }
        }

        public string Name { get; } = "Награждение";

        public string StorylineString { get; }

        public int SceneIndex { get; }

        public string Place { get; }

        public string ActingCharacter { get; }

        public string AdditionalCharacter1 { get; }

        public string AdditionalCharacter2 { get; }
    }
}
