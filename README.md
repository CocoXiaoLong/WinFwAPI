# WinFwAPI

### FirewallRule
```CSharp
public sealed class FirewallRule
{
    public string Name
    public string Description
    public string ApplicationName
    public string ServiceName
    public FirewallProtocol Protocol
    public string LocalPorts
    public string RemotePorts
    public string LocalAddresses
    public string RemoteAddresses
    public FirewallDirection Direction
    public bool Enabled
    public FirewallAction Action
}
```

### Get current rules as an Array
```CSharp
using WinFwAPI;
FirewallRule[] rules = Firewall.GetRules();
```

### Get single Rule
```CSharp
using WinFwAPI;
FirewallRule rule = Firewall.GetRule("%NAME%");
```

