import { Injectable, signal, computed } from '@angular/core';

export enum UnitOfMeasure {
  PerKg,
  Per100g,
  Each,
  Bulb,
  Bunch,
}

export type Offering = {
  name: string;
  priceEur: number;
  uom: UnitOfMeasure;
  imgUrl: string;
};

@Injectable({
  providedIn: 'root',
})
/**
 * Service that maintains list of available offerings and associated shopping cart.
 *
 * Note that in real life, products would be read from a backend service.
 */
export class OfferingsService {
  public readonly offerings: Offering[] = [
    {
      name: 'Almonds',
      priceEur: 10.0,
      uom: UnitOfMeasure.PerKg,
      imgUrl: 'https://i.ibb.co/ZLMvJWQ/Almonds.png',
    },
    {
      name: 'Apple-Red',
      priceEur: 0.5,
      uom: UnitOfMeasure.Each,
      imgUrl: 'https://i.ibb.co/RQWqmqs/Apple-Red.png',
    },
    {
      name: 'Apple-Yellow',
      priceEur: 0.55,
      uom: UnitOfMeasure.Each,
      imgUrl: 'https://i.ibb.co/tDbbLB8/Apple-Yellow.png',
    },
    {
      name: 'Apricot',
      priceEur: 0.6,
      uom: UnitOfMeasure.Each,
      imgUrl: 'https://i.ibb.co/cttLC63/Apricot.png',
    },
    {
      name: 'Aubergine',
      priceEur: 1.2,
      uom: UnitOfMeasure.Each,
      imgUrl: 'https://i.ibb.co/xHCF7Kz/Aubergine.png',
    },
    {
      name: 'Beans',
      priceEur: 2.5,
      uom: UnitOfMeasure.PerKg,
      imgUrl: 'https://i.ibb.co/kDRXPD5/Beans.png',
    },
    {
      name: 'Broccoli',
      priceEur: 1.8,
      uom: UnitOfMeasure.Each,
      imgUrl: 'https://i.ibb.co/VvTC6nZ/Broccoli.png',
    },
    {
      name: 'Carrot',
      priceEur: 0.8,
      uom: UnitOfMeasure.PerKg,
      imgUrl: 'https://i.ibb.co/YpTqCVN/Carrot.png',
    },
    {
      name: 'Cauliflower',
      priceEur: 2.2,
      uom: UnitOfMeasure.Each,
      imgUrl: 'https://i.ibb.co/TK9RWMz/Cauliflower.png',
    },
    {
      name: 'Cherries',
      priceEur: 5.5,
      uom: UnitOfMeasure.PerKg,
      imgUrl: 'https://i.ibb.co/Fmr2419/Cherries.png',
    },
    {
      name: 'Cucumber',
      priceEur: 1.0,
      uom: UnitOfMeasure.Each,
      imgUrl: 'https://i.ibb.co/k34B2Tt/Cucumber.png',
    },
    {
      name: 'Garlic',
      priceEur: 0.3,
      uom: UnitOfMeasure.Bulb,
      imgUrl: 'https://i.ibb.co/Czj7CnC/Garlic.png',
    },
    {
      name: 'Grapes',
      priceEur: 4.0,
      uom: UnitOfMeasure.PerKg,
      imgUrl: 'https://i.ibb.co/BZJ6KgT/Grapes.png',
    },
    {
      name: 'Hazelnut',
      priceEur: 12.0,
      uom: UnitOfMeasure.PerKg,
      imgUrl: 'https://i.ibb.co/MhV7s0B/Hazelnut.png',
    },
    {
      name: 'Jalapeno',
      priceEur: 0.25,
      uom: UnitOfMeasure.Each,
      imgUrl: 'https://i.ibb.co/Z80fpGr/Jalapeno.png',
    },
    {
      name: 'Kale',
      priceEur: 3.0,
      uom: UnitOfMeasure.PerKg,
      imgUrl: 'https://i.ibb.co/P10L479/Kale.png',
    },
    {
      name: 'Lemon',
      priceEur: 0.45,
      uom: UnitOfMeasure.Each,
      imgUrl: 'https://i.ibb.co/0KrZFDb/Lemon.png',
    },
    {
      name: 'Nut',
      priceEur: 10.0,
      uom: UnitOfMeasure.PerKg,
      imgUrl: 'https://i.ibb.co/319bGGc/Nut.png',
    },
    {
      name: 'Olives',
      priceEur: 5.0,
      uom: UnitOfMeasure.Per100g,
      imgUrl: 'https://i.ibb.co/PYVtfF3/Olives.png',
    },
    {
      name: 'Onion',
      priceEur: 0.4,
      uom: UnitOfMeasure.Each,
      imgUrl: 'https://i.ibb.co/71c8r7W/Onion.png',
    },
    {
      name: 'Paprika-Green',
      priceEur: 0.9,
      uom: UnitOfMeasure.Each,
      imgUrl: 'https://i.ibb.co/VB2z92N/Paprika.png',
    },
    {
      name: 'Paprika',
      priceEur: 0.9,
      uom: UnitOfMeasure.Each,
      imgUrl: 'https://i.ibb.co/GpsJ55w/Paprika-Green.png',
    },
    {
      name: 'Pear',
      priceEur: 0.6,
      uom: UnitOfMeasure.Each,
      imgUrl: 'https://i.ibb.co/CKBzkvg/Pear.png',
    },
    {
      name: 'Potato',
      priceEur: 1.5,
      uom: UnitOfMeasure.PerKg,
      imgUrl: 'https://i.ibb.co/Jy7HnzZ/Potato.png',
    },
    {
      name: 'Pumpkin',
      priceEur: 3.5,
      uom: UnitOfMeasure.Each,
      imgUrl: 'https://i.ibb.co/4YJ93NS/Pumpkin.png',
    },
    {
      name: 'Radishes',
      priceEur: 1.2,
      uom: UnitOfMeasure.Bunch,
      imgUrl: 'https://i.ibb.co/Cz31GHk/Radishes.png',
    },
    {
      name: 'Rutabaga',
      priceEur: 1.6,
      uom: UnitOfMeasure.Each,
      imgUrl: 'https://i.ibb.co/VYyXLKr/Rutabaga.png',
    },
    {
      name: 'Shallots',
      priceEur: 2.5,
      uom: UnitOfMeasure.PerKg,
      imgUrl: 'https://i.ibb.co/XYpbcHG/Shallots.png',
    },
    {
      name: 'Tomato',
      priceEur: 1.0,
      uom: UnitOfMeasure.Each,
      imgUrl: 'https://i.ibb.co/3YY8C5h/Tomato.png',
    },
    {
      name: 'Watermelon',
      priceEur: 5.0,
      uom: UnitOfMeasure.Each,
      imgUrl: 'https://i.ibb.co/BfH7Y7t/Watermelon.png',
    },
    {
      name: 'Zuccini',
      priceEur: 1.3,
      uom: UnitOfMeasure.Each,
      imgUrl: 'https://i.ibb.co/ZdFmS9r/Zuccini.png',
    },
  ];

  public shoppingCart = signal<Offering[]>([]);
  public totalPriceEur = computed(() => Math.round(this.shoppingCart().reduce((sum, o) => sum + o.priceEur, 0)));

  public addToCart(offering: Offering): void {
    // Note the use of update here. This is necessary if the signal
    // contains an object or an array and you want to change the inner value.
    this.shoppingCart.update((cart) => {
      const newCart = [...cart];
      const existingOffering = newCart.find((o) => o.name === offering.name);
      if (existingOffering) {
        existingOffering.priceEur += offering.priceEur;
        return newCart;
      }
      newCart.push({ ...offering });
      return newCart;
    });
  }
}
