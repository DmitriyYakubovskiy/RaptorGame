namespace Assets.Scripts.Controllers.Save
{
    public interface ISaveService
    {
        public void Save(SaveData data);

        public SaveData Load();
    }
}
