namespace TorznabClient.Exceptions;

public class TorznabConfigurationException(string message) : TorznabException(-2, message);