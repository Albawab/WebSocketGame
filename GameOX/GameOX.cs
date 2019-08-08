// <copyright file="GameOX.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HenE.Abdul.GameOX
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Sockets;
    using System.Text;
    using HenE.Abdul.Game_OX;
    using HenE.WebSocketExample.Shared.Infrastructure;
    using HenE.WebSocketExample.Shared.Protocol;

    /// <summary>
    /// De class van de Game.
    /// </summary>
    public class GameOX : WebsocketBase
    {
        private readonly StringBuilder vrijVeldenText = new StringBuilder();

        private List<Bord> bords = new List<Bord>();
        private bool nogNietBezit = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameOX"/> class.
        /// </summary>
        /// <param name="dimension">Demension van het bord.</param>
        public GameOX(short dimension)
        {
            this.Dimension = dimension;
            this.Status = GameOXStatussen.NogNietGestart;
        }

        /// <summary>
        /// Gets de list of de spelers.
        /// </summary>
        public IList<Speler> Spelers { get; private set; } = new List<Speler>();

        /// <summary>
        /// Gets de list of clients.
        /// </summary>
        public List<TcpClient> TcpClients { get; private set; } = new List<TcpClient>();

        /// <summary>
        /// Gets de status van het spel.
        /// </summary>
        public GameOXStatussen Status { get; private set; }

        /// <summary>
        /// Gets or sets de dimension van het bord.
        /// </summary>
        public short Dimension { get; set; }

        /// <summary>
        /// Geeft het spel een status.
        /// </summary>
        /// <returns>True or false.</returns>
        public bool IsWaitingForGamers()
        {
            return this.Status == GameOXStatussen.Waiting;
        }

        /// <summary>
        /// Geeft het spel een status.
        /// </summary>
        public void IsGestart()
        {
            this.Status = GameOXStatussen.Gestart;
        }

        /// <summary>
        /// Geef het spel Finish status.
        /// </summary>
        public void EindSpel()
        {
            this.Status = GameOXStatussen.Finished;
        }

        /// <summary>
        /// Add een speler to the play.
        /// </summary>
        /// <param name="naam">De naam van deze speler.</param>
        /// <param name="client">De client van deze speler.</param>
        /// <param name="dimension">De dimension.</param>
        public void AddPlayer(string naam, TcpClient client, short dimension)
        {
            this.Spelers.Add(new HumanSpeler(naam, dimension) { TcpClient = client });
        }

        /// <summary>
        /// Add de copmuter als de speler.
        /// </summary>
        /// <param name="computerNaam">Computer naam.</param>
        /// <param name="dimension">dimension.</param>
        public void AddCpmputerSpeler(string computerNaam, short dimension)
        {
            this.Spelers.Add(new ComputerSpeler(computerNaam, dimension));
        }

        /// <summary>
        /// Add een bord to the play.
        /// </summary>
        /// <param name="dimension">dimension van het bord.</param>
        public void AddBord(short dimension)
        {
            this.bords.Add(new Bord(dimension));
        }

        /// <summary>
        /// Geeft het spel een status.
        /// </summary>
        public void WachtOpAndereSpeler()
        {
            this.Status = GameOXStatussen.Waiting;
        }

        /// <summary>
        /// we hebben genoeg spelers en alle informatie die we nodig hebben, we gaan starten.
        /// </summary>
        /// <param name="tcpClient">De client.</param>
        /// <param name="oX">Het Spel.</param>
        public void Start(TcpClient tcpClient, GameOX oX)
        {
            string bord;
            string msg = string.Empty;

            // maak een bord, met de jusite dimension
            Bord huidigeBord = new Bord(oX.Dimension);
            huidigeBord.ResetBord();

            bord = huidigeBord.TekenBord();
            if (oX.TcpClients.Count == 1)
            {
                msg = string.Format("{0}&{1}", EventHelper.CreateEvents(Events.SpelerGestart), bord);
                this.ProcessStream(msg, tcpClient);
            }
            else
            {
                // hoe bepaal je wie mag beginnen?
                foreach (Speler speler in this.Spelers)
                {
                    if (speler.TcpClient == tcpClient)
                    {
                        msg = string.Format("{0}&{1}", EventHelper.CreateWachtenOpEenAndereDeelnemenEvent(), bord);
                        this.ProcessStream(msg, speler.TcpClient);
                    }
                    else
                    {
                        msg = string.Format("{0}&{1}", EventHelper.CreateEvents(Events.SpelerGestart), bord);
                        this.ProcessStream(msg, speler.TcpClient);
                    }
                }
            }
        }

        /// <summary>
        /// Als de spelers willen een nieuw rondje doen.
        /// </summary>
        /// <param name="tcpClient">Deze client.</param>
        /// <param name="oX">Het Spel.</param>
        public void StartNieuwRondje(TcpClient tcpClient, GameOX oX)
        {
            string bord;
            string msg = string.Empty;
            Bord huidigBord = oX.bords.First();

            huidigBord.ResetBord();
            bord = huidigBord.TekenBord();

            foreach (Speler speler in this.Spelers)
            {
                if (speler.TcpClient == tcpClient)
                {
                    msg = string.Format("{0}&{1}", EventHelper.CreateEvents(Events.YourTurn), bord);

                    this.ProcessStream(msg, speler.TcpClient);
                }
            }
        }

        /// <summary>
        /// Hier de spel is gestart.
        /// </summary>
        /// <param name="tcp">Huidige speler.</param>
        /// <param name="nummer">Wat de speler heeft gekozen.</param>
        /// <param name="gameOX">Het Spel.</param>
        public void SpelGestart(TcpClient tcp, short nummer, GameOX gameOX)
        {
            Bord hetBord = null;
            string tekenBord = string.Empty;
            string msge = string.Empty;
            foreach (Bord bord in gameOX.bords)
            {
                hetBord = bord;
            }

            foreach (Speler speler in gameOX.Spelers)
            {
                // Human Speler tegen computer.
                if (gameOX.TcpClients.Count == 1)
                {
                    if (hetBord.HeeftTekenGewonnen(speler.TeGebruikenTeken))
                    {
                        if (speler.TcpClient != tcp)
                        {
                            tekenBord = hetBord.TekenBord();
                            speler.BeeindigBord();
                            string msg = string.Format("{0}&{1},{2}", EventHelper.CreateEvents(Events.Winnaar), speler.Punten.ToString(), speler.Naam);
                            this.ProcessStream(msg, tcp);
                            msge = string.Format("{0}&{1}", EventHelper.CreateEvents(Events.NieuwRondje), tekenBord);
                            this.ProcessStream(msge, tcp);
                            break;
                        }
                    }
                    else
                    {
                        speler.SpelStartedHandler(nummer, gameOX, hetBord);
                        if (!hetBord.IsBordFinished())
                        {
                            if (speler.TcpClient == tcp)
                            {
                                if (hetBord.HeeftTekenGewonnen(speler.TeGebruikenTeken))
                                {
                                    speler.BeeindigBord();
                                    string msg = string.Format("{0}&{1},{2}", EventHelper.CreateEvents(Events.Winnaar), speler.Punten.ToString(), speler.Naam);
                                    this.ProcessStream(msg, tcp);

                                    tekenBord = hetBord.TekenBord();
                                    msge = string.Format("{0}&{1}", EventHelper.CreateEvents(Events.NieuwRondje), tekenBord);
                                    this.ProcessStream(msge, tcp);
                                    break;
                                }
                            }
                            else
                            {
                                tekenBord = hetBord.TekenBord();
                                string msg = string.Format("{0}&{1}{2}", EventHelper.CreateEvents(Events.YourTurn), tekenBord, this.vrijVeldenText);
                                this.ProcessStream(msg, tcp);
                            }
                        }
                        else
                        {
                            tekenBord = hetBord.TekenBord();
                            msge = string.Format("{0}&{1}", EventHelper.CreateEvents(Events.BordIsVol), tekenBord);
                            this.ProcessStream(msge, tcp);
                            msge = string.Format("{0}&{1}", EventHelper.CreateEvents(Events.NieuwRondje), tekenBord);
                            this.ProcessStream(msge, tcp);
                            break;
                        }
                    }
                }

                // Human speler tegen andre human speler.
                else
                {
                    if (speler.TcpClient == tcp)
                    {
                        speler.SpelStartedHandler(nummer, gameOX, hetBord);
                        tekenBord = hetBord.TekenBord();
                        if (hetBord.HeeftTekenGewonnen(speler.TeGebruikenTeken))
                        {
                            speler.BeeindigBord();
                            msge = string.Format("{0}&{1},{2}", EventHelper.CreateEvents(Events.Winnaar), speler.Punten.ToString(), speler.Naam);
                            this.ProcessReturnMessage(msge, this.TcpClients);
                            msge = string.Format("{0}&{1}", EventHelper.CreateEvents(Events.NieuwRondje), tekenBord);
                            this.ProcessStream(msge, tcp);
                            break;
                        }
                        else if (hetBord.IsBordFinished())
                        {
                            msge = string.Format("{0}&{1}", EventHelper.CreateEvents(Events.NieuwRondje), tekenBord);
                            this.ProcessStream(msge, tcp);
                        }
                        else
                        {
                            msge = string.Format("{0}&{1},{2}", EventHelper.CreateWachtenOpEenAndereDeelnemenEvent(), tekenBord, speler.Naam);
                            this.ProcessStream(msge, speler.TcpClient);
                        }

                        TcpClient tegenHuidigeSpeler;
                        tegenHuidigeSpeler = this.TegenHuidigeSpeler(speler, gameOX);
                        this.vrijVeldenText.Clear();
                        List<short> vrijVelden = hetBord.VrijVelden();
                        foreach (short velder in vrijVelden)
                        {
                            this.vrijVeldenText.AppendFormat("{0},", velder);
                        }

                        if (this.nogNietBezit)
                        {
                            tekenBord = hetBord.TekenBord();
                            string msg = string.Format("{0}&{1}{2}", EventHelper.CreateEvents(Events.YourTurn), tekenBord, this.vrijVeldenText);
                            this.ProcessStream(msg, tegenHuidigeSpeler);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Check als het valid plaats is of niet.
        /// </summary>
        /// <param name="client">De client.</param>
        /// <param name="nummer">Het nummer.</param>
        /// <param name="gameOX">Het Spel.</param>
        public void ChekOfHetValidBeZitIs(TcpClient client, short nummer, GameOX gameOX)
        {
            Bord bord = gameOX.bords.First();
            if (bord.MagZetten(nummer))
            {
                this.SpelGestart(client, nummer, gameOX);
            }
            else
            {
                bord.HetBordIsVol(client, gameOX);
                this.nogNietBezit = false;
            }
        }

        /// <summary>
        /// Als de plaats bezit is.
        /// </summary>
        /// <param name="client">Clietn.</param>
        public void HetIsBezit(TcpClient client)
        {
            string msg = string.Format("{0}", EventHelper.CreateEvents(Events.HetIsBezit));
            this.ProcessStream(msg, client);
        }

        /// <summary>
        /// Bepaalt wie gaat tegen de huidige speler spelen.
        /// </summary>
        /// <param name="huidigeSpeler">De spelers.</param>
        /// <returns>De speler.</returns>
        public Speler TegenSpeler(Speler huidigeSpeler)
        {
            foreach (Speler speler in this.Spelers)
            {
                if (speler != huidigeSpeler)
                {
                    return speler;
                }
            }

            return null;
        }

        /// <summary>
        /// Deze method geef een nieuwe speler als de speler niet al bestaat.
        /// </summary>
        /// <param name="naam">De naam van de human speler.</param>
        /// <param name="teken">welek teken gaat de spelr gebruiken.</param>
        /// <param name="dimension">Dimension.</param>
        /// <returns>Deze method geeft een neuwie speler terug.</returns>
        public Speler AddHumanSpeler(string naam, Teken teken, short dimension)
        {
            Speler speler = new HumanSpeler(naam, dimension)
            {
                TeGebruikenTeken = teken,
            };
            this.Spelers.Add(speler);

            return speler;
        }

        public void Annuleer(Speler doorWie)
        {
            List<TcpClient> tcpClients = new List<TcpClient>();
            foreach (Speler speler in this.Spelers)
            {
                // stuur een bericht naar de overige spelers
                if (speler != doorWie)
                {
                    tcpClients.Add(speler.TcpClient);
                }
            }

            if (tcpClients.Count>0)
            {
                this.ProcessReturnMessage($"{EventHelper.CreateEvents(Events.SpelGeannuleerd)}&{doorWie.Naam}", tcpClients);
            }

            // verwijder de speler
            this.VerwijderSpeler(doorWie);

        }

        
        private void VerwijderSpeler(Speler wie)
        {
            this.Spelers.Remove(wie);
            if (this.Spelers.Count > 0)
            {
                // zet de status op wachten
                this.Status = GameOXStatussen.Waiting;
            }
            else
            {
                this.Status = GameOXStatussen.Finished;
            }
        }


        /// <summary>
        /// Als het spel klaar is .
        /// </summary>
        /// <param name="oX">Het Spel.</param>
        /// <param name="tcp">De client tcp handler.</param>
        public void BeeidigSpel(TcpClient tcpClient)
        {
            string message = string.Empty;
            foreach (Speler speler in this.Spelers)
            {
                //speler.HetSpelIsBeeindigd(tcpClient);

                if (TegenSpeler(speler).TcpClient.Connected && tcpClient.Connected)
                {
                    if (speler.Punten > this.TegenSpeler(speler).Punten)
                    {
                        message = string.Format("{0}&{1}", EventHelper.CreateEvents(Events.IsGewonnen), speler.Naam);
                        this.ProcessReturnMessage(message, this.TcpClients);
                    }
                    else if (speler.Punten == this.TegenSpeler(speler).Punten)
                    {
                        message = string.Format("{0}&{1}", EventHelper.CreateEvents(Events.NiemandGewonnen), speler.Naam);
                        ProcessReturnMessage(message, this.TcpClients);
                    }
                }
            }

            this.EindSpel();
        }

        /// <summary>
        /// Deze method zoekt of de naam van de niuwe speler al bastaat.
        /// </summary>
        /// <param name="speler">de speler.</param>
        /// <returns>De niuwe speler.</returns>
        public string FindSpelerByNaam(Speler speler)
        {
                if (this.TegenSpeler(speler).Naam == speler.Naam)
                {
                    return speler.Naam;
                }

                return null;
        }

        /// <summary>
        /// Process Stream die naar de clinet gaat.
        /// </summary>
        /// <param name="stream">De stream.</param>
        /// <param name="client">Naar deze client.</param>
        /// <returns>Message.</returns>
        protected override string ProcessStream(string stream, TcpClient client)
        {
            string returnMessage = null;
            this.ProcessReturnMessage(stream, client);

            return returnMessage;
        }

        /// <summary>
        /// Met deze method kan in de code inloggen.
        /// </summary>
        /// <param name="huidigeSpeler">Huidige spelr.</param>
        /// <param name="games">huidige game.</param>
        /// <returns>Tegen deze speler.</returns>
        private TcpClient TegenHuidigeSpeler(Speler huidigeSpeler, GameOX games)
        {
            foreach (Speler speler in games.Spelers)
            {
                if (speler != huidigeSpeler)
                {
                    return speler.TcpClient;
                }
            }

            return null;
        }
    }
}
