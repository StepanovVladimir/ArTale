using Assets.Scripts.ProppMorphology.CharacterFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.ProppMorphology
{
    class CharacterFunctionsSequenceGenerator
    {
        private Random rnd = new Random();

        public List<ICharacterFunction> Generate()
        {
            var characterFunctions = new List<ICharacterFunction>();

            TaleKind taleKind;
            string initialPlace;
            string antagonistsPlace;
            string intermediatePlace = "Лес";
            string finalLocation;

            var characters = new Characters();

            //if (rnd.Next(0, 2) == 1)
            //{
                taleKind = TaleKind.SnakeKidnapsPrincess;
                initialPlace = "Царский сад";
                antagonistsPlace = "Логово трёхглавого змея";
                finalLocation = "Царский дворец";

                int randomValue = rnd.Next(0, 4);
                if (randomValue == 0)
                {
                    characters.Hero = "Иван";
                }
                else if (randomValue == 1)
                {
                    characters.Hero = "Фролка";
                }
                else if (randomValue == 2)
                {
                    characters.Hero = "Зорька";
                }
                else
                {
                    characters.Hero = "Никита кожемяка";
                }

                characters.Victim = "Царевна";
                characters.Parent = "Царь";
                characters.Antagonist1 = "Трёхглавый змей";
                characters.Antagonist2 = "Змеиха";
            /*}
            else
            {
                initialPlace = "Двор дома";
                antagonistsPlace = "Избушка на курьих ножках";
                finalLocation = "Дом";

                if (rnd.Next(0, 2) == 1)
                {
                    taleKind = TaleKind.SisterSavesBrotherFromBabaYaga;
                    characters.Hero = "Сестра";
                    characters.Victim = "Братец";
                    characters.Antagonist1 = "Гуси-лебеди";
                    characters.Antagonist2 = "Баба-яга";
                }
                else
                {
                    taleKind = TaleKind.BoyEscapesFromBabaYaga;
                    if (rnd.Next(0, 2) == 1)
                    {
                        characters.Hero = "Терешечка";
                    }
                    else
                    {
                        characters.Hero = "Ивашко";
                    }
                    characters.Antagonist1 = "Баба-яга";
                    characters.Antagonist2 = "Дочь бабы-яги";
                }

                characters.Parent = "Старик и старушка";
            }*/

            characterFunctions.Add(new InitialSituationFunction(taleKind, initialPlace, characters));
            characterFunctions.Add(new VillainyFunction(taleKind, initialPlace, characters));

            if (taleKind != TaleKind.BoyEscapesFromBabaYaga)
            {
                characterFunctions.Add(new TroubleMessageFunction(taleKind, initialPlace, characters));
                characterFunctions.Add(new CounteractionDecisionFunction(taleKind, initialPlace, characters));
            }

            bool itWasTravelGuide = false;
            if (taleKind != TaleKind.BoyEscapesFromBabaYaga)
            {
                if (rnd.Next(0, 2) == 1)
                {
                    itWasTravelGuide = true;
                }

                /*if (rnd.Next(0, 2) == 1)
                {
                    departureFunction.Place = intermediatePlace;
                    characterFunctions.Add(new TestByDonorFunction { Place = intermediatePlace });
                    characterFunctions.Add(new PassingTestFunction { Place = intermediatePlace });
                    if (rnd.Next(0, 2) == 1)
                    {
                        characterFunctions.Add(new MagicAgentTransferFunction { Place = intermediatePlace });
                    }
                    else
                    {
                        if (taleKind == TaleKind.SnakeKidnapsPrincess)
                        {
                            characterFunctions.Add(new TravelGuideFunction(taleKind, antagonistsPlace, characters, "Старик"));
                        }
                        else
                        {
                            characterFunctions.Add(new TravelGuideFunction(taleKind, antagonistsPlace, characters, "Ёжик"));
                        }
                    }
                }
                else if (rnd.Next(0, 2) == 1)
                {
                    departureFunction.Place = intermediatePlace;
                    if (rnd.Next(0, 2) == 1)
                    {
                        characterFunctions.Add(new MagicAgentTransferFunction { Place = intermediatePlace });
                    }
                    else
                    {
                        if (taleKind == TaleKind.SnakeKidnapsPrincess)
                        {
                            characterFunctions.Add(new TravelGuideFunction(taleKind, antagonistsPlace, characters, "Старик"));
                        }
                        else
                        {
                            characterFunctions.Add(new TravelGuideFunction(taleKind, antagonistsPlace, characters, "Ёжик"));
                        }
                    }
                }

                if (departureFunction.Place == intermediatePlace) // И если не было путеводительства
                {
                    characterFunctions.Add(new DepartureFunction(taleKind, antagonistsPlace, characters));
                }
                */
            }

            if (!itWasTravelGuide)
            {
                characterFunctions.Add(new DepartureFunction(taleKind, antagonistsPlace, characters, itWasTravelGuide));
            }
            else
            {
                characterFunctions.Add(new DepartureFunction(taleKind, intermediatePlace, characters, itWasTravelGuide));
                if (taleKind == TaleKind.SnakeKidnapsPrincess)
                {
                    characterFunctions.Add(new TravelGuideFunction(taleKind, antagonistsPlace, characters, "Старик"));
                }
                else
                {
                    characterFunctions.Add(new TravelGuideFunction(taleKind, antagonistsPlace, characters, "Ёжик"));
                }
            }

            if (taleKind != TaleKind.SisterSavesBrotherFromBabaYaga)
            {
                characterFunctions.Add(new StruggleFunction(taleKind, antagonistsPlace, characters));
                characterFunctions.Add(new VictoryFunction(taleKind, antagonistsPlace, characters));

                if (taleKind == TaleKind.SnakeKidnapsPrincess)
                {
                    characterFunctions.Add(new TroublesLiquidationFunction(taleKind, antagonistsPlace, characters));
                }

                if (taleKind == TaleKind.BoyEscapesFromBabaYaga)
                {
                    characterFunctions.Add(new PursuitFunction(taleKind, intermediatePlace, characters));
                    characterFunctions.Add(new RescueFromPursuitFunction(taleKind, intermediatePlace, characters, "Гусь-лебедь"));
                }
                else if (rnd.Next(0, 2) == 1)
                {
                    characterFunctions.Add(new PursuitFunction(taleKind, intermediatePlace, characters));
                    characterFunctions.Add(new RescueFromPursuitFunction(taleKind, "Кузница", characters, "Кузнец"));
                }
            }
            else
            {
                /*bool itWasStruggle = false;
                if (rnd.Next(0, 2) == 1)
                {
                    itWasStruggle = true;
                    characterFunctions.Add(new StruggleFunction(taleKind, antagonistsPlace, characters));
                    characterFunctions.Add(new VictoryFunction(taleKind, antagonistsPlace, characters));
                }*/

                characterFunctions.Add(new TroublesLiquidationFunction(taleKind, antagonistsPlace, characters));

                characterFunctions.Add(new PursuitFunction(taleKind, intermediatePlace, characters));
                characterFunctions.Add(new RescueFromPursuitFunction(taleKind, intermediatePlace, characters, "Яблоня"));
            }

            characterFunctions.Add(new ReturnFunction(taleKind, finalLocation, characters));

            if (taleKind == TaleKind.SnakeKidnapsPrincess)
            {
                if (true/*rnd.Next(0, 2) == 1*/)
                {
                    characterFunctions.Add(new RewardingFunction(taleKind, finalLocation, characters, true));
                }
                else
                {
                    //characterFunctions.Add(new RewardingFunction(taleKind, finalLocation, characters, false));
                }
            }

            return characterFunctions;
        }
    }
}
