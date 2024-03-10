namespace StreamGame.Entities;

public static class EntityManager
{
    static List<Entity> entities = new List<Entity>();
    static List<Entity> addedEntities = new List<Entity>();

    static bool isUpdating = false;

    public static int Count => entities.Count;

    public static void Add(Entity entity)
    {
        if (!isUpdating)
        {
            entities.Add(entity);
            return;
        }

        addedEntities.Add(entity);
    }

    public static void Update()
    {
        isUpdating = true;

        //Update all entities
        foreach (Entity entity in entities)
        {
            entity.Update();
        }

        isUpdating = false;

        //Add all new entities
        foreach (Entity entity in addedEntities)
        {
            entities.Add(entity);
        }

        //Clear the added entities
        addedEntities.Clear();

        //Remove expired entities
        //entities.RemoveAll(entity => entity.IsExpired);
    }

    public static void Draw(SpriteBatch spriteBatch)
    {
        foreach (Entity entity in entities)
        {
            entity.Draw(spriteBatch);
        }
    }
}
