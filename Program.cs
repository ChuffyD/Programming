using System;
using System.Linq;
// --- ИСПОЛНЯЕМЫЙ КОД ---

MeleeWeapon newKnife = new MeleeWeapon();
Loadout newLoadout = new Loadout();

try
{
    newLoadout.AttachOptic(newKnife);
}
catch (InvalidAttachmentException ex)
{
    Console.WriteLine($"Ошибка: {ex.Message}");
}
Lootbox<MeleeWeapon> Lootbox = new Lootbox<MeleeWeapon>();
Lootbox.AddItem(newKnife);
Lootbox.OpenBox();
Marketplace marketplace = new Marketplace();
WeaponSkin skin1 = new WeaponSkin { Name = "Oxy", SkinRarity = Rarity.Restricted, Wear = 0.15 };
WeaponSkin skin2 = new WeaponSkin { Name = "Asiimov", SkinRarity = Rarity.Covert, Wear = 0.25 };
WeaponSkin skin3 = new WeaponSkin { Name = "Sand Dune", SkinRarity = Rarity.MilSpec, Wear = 0.60 };
marketplace.Skins.Add(skin1);
marketplace.Skins.Add(skin2);
marketplace.Skins.Add(skin3);
skin1.Price = 500m;
skin2.Price = 200m;
skin3.Price = 400m;
Player myPlayer = new Player { Balance = 1000m };
try { myPlayer.BuySkin(marketplace, "Oxy"); }
catch (SkinNotFoundException ex) { Console.WriteLine($"Ошибка поиска: {ex.Message}"); }
catch (NotEnoughMoneyException ex) { Console.WriteLine($"Ошибка покупки: {ex.Message}"); }
var res = marketplace.Skins.Where(s => s.SkinRarity == Rarity.Restricted);
var sorted = marketplace.Skins.OrderBy(s => s.Wear);
var FirstMil = marketplace.Skins.FirstOrDefault(s => s.SkinRarity == Rarity.MilSpec);
foreach (var skin in sorted)
{
    Console.WriteLine($"{skin.Name} (Износ: {skin.Wear})");
}
foreach (var mil in res)
{
    Console.WriteLine(mil.Name);
}
if (FirstMil != null) { Console.WriteLine(FirstMil.Name); }

StatTrakSkin unskin = new StatTrakSkin();
unskin.Addkill();
Console.WriteLine($"Количество килов: {unskin.Kills}");









// --- ОПРЕДЕЛЕНИЯ КЛАССОВ И ИНТЕРФЕЙСОВ ---

public enum Rarity
{
    MilSpec, Restricted, Covert
}

public class WeaponSkin
{
    public string Name { get; set; }
    public Rarity SkinRarity { get; set; }

    private double _wear;
    public double Wear
    {
        get { return _wear; }
        set
        {
            if (value >= 0.0 && value <= 1.0)
                _wear = value;
        }
    }
    public decimal Price {  get; set; }
}

public struct WeaponStats
{
    public double Damage { get; set; }
    public double FireRate { get; set; }
}

public class Loadout
{
    private WeaponSkin[] _slots = new WeaponSkin[5];

    public WeaponSkin this[int index]
    {
        get
        {
            if (index >= 0 && index < 5)
            {
                return _slots[index];
            }
            else
            {
                return null;
            }
        }
        set
        {
            if (index >= 0 && index < 5)
            {
                _slots[index] = value;
            }
        }
    }

    public void AttachOptic(IEquippable item)
    {
        if (item is MeleeWeapon)
        {
            throw new InvalidAttachmentException("На нож не вешают прицел");
        }
        else
        {
            Console.WriteLine("Прицел успешно установлен");
        }
    }
}

public interface IEquippable
{
    void Equip();
}

public interface IInspectable
{
    void Inspect();
}

public class AssaultRifle : IEquippable, IInspectable
{
    public void Equip()
    {
        Console.WriteLine("Достаем штурмовую винтовку, снимаем с предохранителя.");
    }

    public void Inspect()
    {
        Console.WriteLine("Осматриваем винтовку: проверяем затвор и магазин.");
    }
}

public class MeleeWeapon : IEquippable, IInspectable
{
    public void Equip()
    {
        Console.WriteLine("Достаем ножик");
    }

    public void Inspect()
    {
        Console.WriteLine("Крутим вертим");
    }
}

public class InvalidAttachmentException : Exception
{
    public InvalidAttachmentException(string message) : base(message) { }
}
public class Lootbox<T> where T : IEquippable
{
    private List<T> _items = new List<T>();
    public void AddItem(T item)
    {
        _items.Add(item);
    }
    public T OpenBox()
    {
        if (_items.Count == 0)
        { return default(T); }
        T wonItem = _items[0];
        _items.RemoveAt(0);
        wonItem.Equip();
        return wonItem;
    }
}
public class Marketplace
{
    public List<WeaponSkin> Skins { get; set; } = new List<WeaponSkin>();
}
public class StatTrakSkin : WeaponSkin
{
    public int Kills { get; private set; }
    public void Addkill()
    {
        Kills++;
    }
}
public class Player
{
    public decimal Balance { get; set; }
    public List<WeaponSkin> Inventory = new List<WeaponSkin>();
    public void BuySkin(Marketplace market, string skinName)
    {
        var targetSkin = market.Skins.FirstOrDefault(x => x.Name == skinName);
        if (targetSkin == null)
        {
            throw new SkinNotFoundException("Скин не найден");
        }
        if (Balance < targetSkin.Price)
        {
            throw new NotEnoughMoneyException("Мало денег");
        }
        Balance -= targetSkin.Price;
        Inventory.Add(targetSkin);
        market.Skins.Remove(targetSkin);
    }
}
public class NotEnoughMoneyException : Exception
{
    public NotEnoughMoneyException(string message) : base(message) { }
}
public class SkinNotFoundException : Exception
{
    public SkinNotFoundException(string message) : base(message) { }
}
public static class TradeUpContract
{
    public static WeaponSkin Craft(List<WeaponSkin> ingredients)
    {





        if (ingredients.Count != 10)
        {
            throw new ArgumentException("Нужно ровно 10 скинов!");
        }
        Rarity targetRarity = ingredients[0].SkinRarity;
        bool isSameRarity = ingredients.All(s => s.SkinRarity == targetRarity);
        if (!isSameRarity)
        {
            throw new ArgumentException("Все предметы должны быть одной редкости.");
        }
        if (targetRarity == Rarity.Covert) {
            throw new InvalidOperationException("Нельзя скрафтить скин выше редкости Covert.");
        }
        Rarity newRarity = Rarity.MilSpec;
        if (targetRarity == Rarity.MilSpec)
        {
            newRarity = Rarity.Restricted;
        }
        else if (targetRarity == Rarity.Restricted)
        {
            newRarity = Rarity.Covert;
        }
        return new WeaponSkin
        {
            Name = "Dragon Lore",
            SkinRarity = Rarity.Covert, // Тут потом сделаешь вычисление новой редкости
            Wear = 0.01
        };
    }
}   