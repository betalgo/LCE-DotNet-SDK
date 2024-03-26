using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace LaserCatEyes.WCFListener;

public class LaserCatEyesEndpointBehaviour(IClientMessageInspector messageInspector) : IEndpointBehavior
{
    private IClientMessageInspector MessageInspector { get; } = messageInspector ?? throw new ArgumentNullException(nameof(messageInspector));

    public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
    {
    }

    public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
    {
        clientRuntime.ClientMessageInspectors.Add(MessageInspector);
    }

    public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
    {
    }

    public void Validate(ServiceEndpoint endpoint)
    {
    }
}