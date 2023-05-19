namespace MSuhininTestovoe.B2B
{
    public enum GameObjectsTypeId
    {
        //Game objects which index less 100 would be used in pool
        //Obstacle = 1,
        Coin = 2,
        PlatformWithoutSteps = 3,
        PillarLamp = 4,
        WallLamp = 5,
        ObjectSpawner = 6,
        Wrench = 7,// что то типа ключа
        
        //Ease tunnels 21-30
        ETunnel = 21,
        //Medium tunnels 31-40
        MTunnel = 31,
        //Hard tunnels 41-51
        HTunnel = 41,
        //Game object not used in pool
        PlatformWithSteps = 101,
        Platform = 102,
        //Markers
        
    } 
}