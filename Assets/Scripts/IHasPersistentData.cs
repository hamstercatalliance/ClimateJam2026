public interface IHasPersistentData
{
    void WriteToGameData();
    void LoadGameData();
    bool DataSuccessfullyWritten { get; }
}
