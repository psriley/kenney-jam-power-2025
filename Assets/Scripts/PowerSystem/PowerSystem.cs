using System;
using System.Collections.Generic;
using UnityEngine;

public class PowerSystem
{
    public PowerStorage ps;

    public List<IPowerProducer> powerProducers;
    public List<IPowerConsumer> powerConsumers;
    private int constantDrain = 0;

    // public int Timer;

    public PowerSystem(PowerStorage _ps, List<IPowerProducer> _powerProducers, List<IPowerConsumer> _powerConsumers)
    {
        ps = _ps;
        powerProducers = _powerProducers;
        powerConsumers = _powerConsumers;
    }

    public void Tick() 
    {
        foreach (IPowerProducer p in powerProducers)
        {
            ps.Store(p.Produce);
        }
        foreach (IPowerConsumer p in powerConsumers)
        {
            ps.Drain(p.Consume);
        }

        ps.Drain(constantDrain);
    }
}
