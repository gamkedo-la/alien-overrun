/**
 * Description: Types and interactions - for combat and collisions.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

public enum DamageType
{
	Physical,
	Magical,
	Fire,
}

public enum ResistanceType
{
	Normal,
	Armored,
	Flesh,
}

static public class Interactions
{
	static public float GetMultiplier( DamageType damage, ResistanceType resistance )
	{
		// Special cases for Physical damage vs. different resistances
		if ( damage == DamageType.Physical )
		{
			if ( resistance == ResistanceType.Armored )
				return 0.5f;
		}

		// Special cases for Magical damage vs. different resistances
		if ( damage == DamageType.Magical )
		{
			if ( resistance == ResistanceType.Armored )
				return 2.0f;
		}

		// Special cases for Fire damage vs. different resistances
		if ( damage == DamageType.Fire )
		{
			if ( resistance == ResistanceType.Flesh )
				return 2.0f;
		}

		// Default for non-special cases
		return 1.0f;
	}
}
