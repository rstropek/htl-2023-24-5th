import { Injectable, computed, signal } from '@angular/core';
import { Offering } from './shopping.service';

@Injectable({
  providedIn: 'root'
})
export class ShoppingCartService {
  public shoppingCart = signal<Offering[]>([]);

  public totalPrice = computed(() => {
    let sum = 0;
    for (const offering of this.shoppingCart()) {
      sum += offering.priceEur;
    }

    return Math.round(sum);
  });
  public totalPriceWithReduce = computed(() => this.shoppingCart().reduce((sum, o) => sum + o.priceEur, 0));

  public addToCart(offering: Offering): void {
    this.shoppingCart.update((cart) => {
      const existingOffering = cart.find((o) => o.name === offering.name);
      if (existingOffering) {
        existingOffering.priceEur += offering.priceEur;
        return cart;
      }

      cart.push({ ...offering });
      return cart;
    });
  }
}
