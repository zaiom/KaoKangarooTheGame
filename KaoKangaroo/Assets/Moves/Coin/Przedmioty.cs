using UnityEngine;
[CreateAssetMenu(menuName = "Przedmioty")]
public class Przedmioty : ScriptableObject
{
public string nazwaPrzedmiotu;
public Sprite ikonka;
public int ilosc;
public bool doGromadzenia;
public enum RodzajPrzedmiotu
{
MONETA,
ZYCIE,
INNEBADZIEWIE
}
public RodzajPrzedmiotu rodzajPrzedmiotu;
}