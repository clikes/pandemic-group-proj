using System.Collections;
using System.Collections.Generic;
using UnityEngine;

	
	public class DeckManager : MonoBehaviour {

		private List<PlayerCard> PlayerCardDeck = new List<PlayerCard> (new PlayerCard[] {
			PlayerCard.Algiers,
			PlayerCard.Atlanta,
			PlayerCard.Baghdad,
			PlayerCard.Bangkok,
			PlayerCard.Beijing,
			PlayerCard.Bogota,
			PlayerCard.BuenosAries,
			PlayerCard.Cairo,
			PlayerCard.Chennai,
			PlayerCard.Chicago,
			PlayerCard.Delhi,
			PlayerCard.Essen,
			PlayerCard.HoChiMinhCity,
			PlayerCard.HongKong,
			PlayerCard.Istanbul,
			PlayerCard.Jakarta,
			PlayerCard.Johannesburg,
			PlayerCard.Karachi,
			PlayerCard.Khartoum,
			PlayerCard.Kinshasa,
			PlayerCard.Kolkata,
			PlayerCard.Lagos,
			PlayerCard.Lima,
			PlayerCard.London,
			PlayerCard.LosAngeles,
			PlayerCard.Madrid,
			PlayerCard.Manila,
			PlayerCard.MexicoCity,
			PlayerCard.Miami,
			PlayerCard.Milan,
			PlayerCard.Montreal,
			PlayerCard.Moscow,
			PlayerCard.Mumbai,
			PlayerCard.NewYork,
			PlayerCard.Osaka,
			PlayerCard.Paris,
			PlayerCard.Riyadh,
			PlayerCard.SanFrancisco,
			PlayerCard.Santiago,
			PlayerCard.SaoPaulo,
			PlayerCard.Seoul,
			PlayerCard.Shanghai,
			PlayerCard.StPetersburg,
			PlayerCard.Sydney,
			PlayerCard.Taipei,
			PlayerCard.Tehran,
			PlayerCard.Tokyo,
			PlayerCard.Washington
		});
		private List<PlayerCard> PlayerDiscardPile = new List<PlayerCard> ();
		private List<InfectionCard> InfectionCardDeck = new List<InfectionCard> (new InfectionCard[] {
			InfectionCard.Algiers,
			InfectionCard.Atlanta,
			InfectionCard.Baghdad,
			InfectionCard.Bangkok,
			InfectionCard.Beijing,
			InfectionCard.Bogota,
			InfectionCard.BuenosAries,
			InfectionCard.Cairo,
			InfectionCard.Chennai,
			InfectionCard.Chicago,
			InfectionCard.Delhi,
			InfectionCard.Essen,
			InfectionCard.HoChiMinhCity,
			InfectionCard.HongKong,
			InfectionCard.Istanbul,
			InfectionCard.Jakarta,
			InfectionCard.Johannesburg,
			InfectionCard.Karachi,
			InfectionCard.Khartoum,
			InfectionCard.Kinshasa,
			InfectionCard.Kolkata,
			InfectionCard.Lagos,
			InfectionCard.Lima,
			InfectionCard.London,
			InfectionCard.LosAngeles,
			InfectionCard.Madrid,
			InfectionCard.Manila,
			InfectionCard.MexicoCity,
			InfectionCard.Miami,
			InfectionCard.Milan,
			InfectionCard.Montreal,
			InfectionCard.Moscow,
			InfectionCard.Mumbai,
			InfectionCard.NewYork,
			InfectionCard.Osaka,
			InfectionCard.Paris,
			InfectionCard.Riyadh,
			InfectionCard.SanFrancisco,
			InfectionCard.Santiago,
			InfectionCard.SaoPaulo,
			InfectionCard.Seoul,
			InfectionCard.Shanghai,
			InfectionCard.StPetersburg,
			InfectionCard.Sydney,
			InfectionCard.Taipei,
			InfectionCard.Tehran,
			InfectionCard.Tokyo,
			InfectionCard.Washington
		});
    private List<InfectionCard> InfectionDiscardPile = new List<InfectionCard>();
    public void ShuffleCardInitcialize(int seed){
		Shuffle(PlayerCardDeck,seed);
        Shuffle(InfectionCardDeck, seed);
    }
    private void Shuffle<T>(List<T> deck, int seed)
    {
        int n = deck.Count;
        System.Random rng = new System.Random(seed);
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = deck[k];
            deck[k] = deck[n];
            deck[n] = value;
        }
    }
   

    public PlayerCard DrawPlayerCard()
    {
        PlayerCard tmp = PlayerCardDeck[0];
        PlayerCardDeck.Remove(tmp);
        return tmp;
    }
    public InfectionCard DrawInfectionCard()
    {
        InfectionCard tmp = InfectionCardDeck[0];
        InfectionCardDeck.Remove(tmp);
        return tmp;
    }

    public void PutInDiscardPile(PlayerCard card)
    {
        PlayerDiscardPile.Add(card);
    }

    public void PutInDiscardPile(InfectionCard card)
    {
        InfectionDiscardPile.Add(card);
    }

    public InfectionCard RemoveFromBottom()
    {
        int tmp = InfectionCardDeck.Count - 1;
        InfectionCard bottomCard = InfectionCardDeck[tmp];

        PutInDiscardPile(bottomCard);
        InfectionCardDeck.RemoveAt(tmp);

        return bottomCard;
    }

    public void DiscardPileOnTop()
    {

        int seed = Random.Range(0, 1000);
        Shuffle(InfectionDiscardPile, seed);
        foreach (InfectionCard card in InfectionCardDeck)
        {
            InfectionDiscardPile.Add(card);
        }
        InfectionCardDeck = InfectionDiscardPile;
        InfectionDiscardPile = new List<InfectionCard>();
    }

    private void Awake()
    {
        for (int i = 0; i < 48; i++)
        {
            GameObject.Find(PlayerCardDeck[i].ToString()).GetComponent<City>().cityNumber = i+1;
            Debug.Log(PlayerCardDeck[i].GetHashCode() + " " + (i+1));
        }
    }
    void Start () {
			//Shuffle (PlayerCardDeck);
			// for (int i = 0; i < 48; i++) {
			// 	GameObject.Find (PlayerCardDeck[i].ToString ()).SetActive(false);
			// }
        
	}

		// Update is called once per frame
		void Update () {

		}
	}
