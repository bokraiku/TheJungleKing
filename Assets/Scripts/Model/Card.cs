using UnityEngine;
using System.Collections;

public class Card {
	public string card;
	public string ID;
	public bool swipe;
	public bool shuffle;

	public Card() {
	}

	public Card(string ID, bool swipe) {
		this.ID = ID;
		this.swipe = swipe;
		this.shuffle = true;
	}

}
