using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.ProppMorphology.CharacterFunctions
{
    class TroublesLiquidationFunction : ICharacterFunction
    {
        public TroublesLiquidationFunction(TaleKind taleKind, string place, Characters characters)
        {
            SceneIndex = 8;
            Place = place;
            ActingCharacter = characters.Hero;
            AdditionalCharacter1 = characters.Victim;
            if (taleKind == TaleKind.SnakeKidnapsPrincess)
            {
                StorylineString = $"{characters.Hero} освобождает царевну";
            }
            else
            {
                StorylineString = "Сестра, подкравшись, освобождает братца от бабы-яги";
            }
        }

        public string Name { get; } = "Ликвидация беды";

        public string StorylineString { get; }

        public int SceneIndex { get; }

        public string Place { get; }

        public string ActingCharacter { get; }

        public string AdditionalCharacter1 { get; }

        public string AdditionalCharacter2 { get; }
    }
}
