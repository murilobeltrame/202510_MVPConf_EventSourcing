namespace Domain.ValueObjects;

public class Species(string name, string scientificName, IEnumerable<Prey> hunts, IEnumerable<Food> collects)
{
    public string Name { get; private set; } = name;
    public string ScientificName { get; private set; } = scientificName;
    public IEnumerable<Prey> Hunts { get; private set; } = hunts;
    public IEnumerable<Food> Collects { get; private set; } = collects;
    
    public static IEnumerable<Species> List =>
    [
        Marten,
        Otter,
        RiverOtter,
        SeaOtter,
        Weasel,
        Stoat,
        Badger,
        Ferret,
        Mink,
        Polecat,
        Wolverine
    ];
    
    public static Species Marten => new("Marten", "Martes martes", [
        Prey.SmallMammals,
        Prey.Squirrels,
        Prey.Voles,
        Prey.Mice,
        Prey.Birds,
        Prey.Insects
    ], [
        Food.Eggs,
        Food.Berries,
        Food.Carrion
    ]);
    
    public static Species Otter => new("Otter", "Lutra lutra", [
        Prey.Fish,
        Prey.Frogs,
        Prey.Crayfish,
        Prey.Amphibians
    ],[]);
    
    public static Species RiverOtter => new ("River Otter", "Lontra canadensis", [
        Prey.Fish,
        Prey.Crustaceans,
        Prey.Amphibians,
    ],[]);
    
    public static Species SeaOtter => new ("Sea Otter", "Enhydra lutris", [
        Prey.Urshins,
        Prey.Mussels,
        Prey.Abalone,
        Prey.Crabs,
        Prey.Fish
    ],[
        Food.Clams
    ]);
    
    public static Species Weasel => new("Weasel", "Mustela nivalis", [
        Prey.SmallMammals,
        Prey.Rats,
        Prey.Mice,
        Prey.Voles,
        Prey.Rabbits,
        Prey.Hares,
        Prey.Birds,
        Prey.Insects
    ],[
        Food.Eggs,
        Food.Carrion
    ]);
    
    public static Species Stoat => new("Stoat", "Mustela erminea", [
        Prey.SmallMammals,
        Prey.Rats,
        Prey.Mice,
        Prey.Voles,
        Prey.Rabbits,
        Prey.Hares,
        Prey.Birds,
        Prey.Insects
    ],[
        Food.Eggs,
        Food.Carrion
    ]);
    
    public static Species Badger => new("Badger", "Meles meles", [
        Prey.SmallMammals,
        Prey.Rats,
        Prey.Mice,
        Prey.Voles,
        Prey.Insects,
        Prey.Earthworms,
        Prey.Crayfish
    ],[
        Food.Cereals,
        Food.Fruits,
        Food.Berries,
        Food.Carrion
    ]);
    
    public static Species Ferret => new("Ferret", "Mustela putorius furo", [
        Prey.SmallMammals,
        Prey.Rats,
        Prey.Mice,
        Prey.Voles,
        Prey.Rabbits,
        Prey.Hares,
        Prey.Birds,
        Prey.Insects
    ],[
        Food.Eggs,
        Food.Carrion
    ]);
    
    public static Species Mink => new("European Mink", "Mustela lutreola", [
        Prey.Fish,
        Prey.Crayfish,
        Prey.Amphibians,
        Prey.SmallMammals,
        Prey.Birds
    ],[
        Food.Eggs,
        Food.Carrion
    ]);
    
    public static Species Polecat => new("Polecat", "Mustela putorius", [
        Prey.SmallMammals,
        Prey.Rats,
        Prey.Mice,
        Prey.Voles,
        Prey.Rabbits,
        Prey.Hares,
        Prey.Birds,
        Prey.Insects
    ],[
        Food.Eggs,
        Food.Carrion
    ]);
    
    public static Species Wolverine => new("Wolverine", "Gulo gulo", [
        Prey.LargeMammals,
        Prey.Deer,
        Prey.Caribou,
        Prey.Hares,
        Prey.Rabbits,
        Prey.Birds,
        Prey.Fish,
    ],[
        Food.Berries,
        Food.Honey,
        Food.Cereals,
        Food.Carrion
    ]);
}