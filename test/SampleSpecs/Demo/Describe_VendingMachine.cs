﻿#region [R# naming]

// ReSharper disable ArrangeTypeModifiers
// ReSharper disable UnusedMember.Local
// ReSharper disable FieldCanBeMadeReadOnly.Local
// ReSharper disable ArrangeTypeMemberModifiers
// ReSharper disable InconsistentNaming

#endregion

using NSpectator;
using SampleSpecs.Model;
using FluentAssertions;

namespace SampleSpecs.Demo
{
    class describe_VendingMachine : Spec
    {
        VendingMachine vendingMachine = null;

        void before_each()
        {
            vendingMachine = new VendingMachine();
        }

        void when_stocking_vending_machine_with_chips()
        {
            act = () => vendingMachine.AddInventory("chips");

            it["should contain chips with count of 1"] = () => vendingMachine.Inventory("chips").Should().Be(1);

            context["multiple chips added"] = () =>
            {
                act = () => vendingMachine.AddInventory("chips");

                it["should increment chip inventory with count of 2"] = () => vendingMachine.Inventory("chips").Should().Be(2);
            };
        }

        void when_buying_an_item()
        {
            context["vending maching has inventory"] = () =>
            {
                before = () =>
                {
                    vendingMachine.AddInventory("chips");
                    vendingMachine.PricePoint("chips", .5);
                };

                act = () => vendingMachine.Buy("chips");

                it["should decrement inventory"] = () => vendingMachine.Inventory("chips").Should().Be(0);

                it["should increment cash in machine"] = () => vendingMachine.Cash.Should().Be(.5);
            };
        }
    }
}