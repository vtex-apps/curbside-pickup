📢 Use this project, [contribute](https://github.com/vtex-apps/curbside-pickup) to it or open issues to help evolve it using [Store Discussion](https://github.com/vtex-apps/store-discussion).

<!-- ALL-CONTRIBUTORS-BADGE:START - Do not remove or modify this section -->

[![All Contributors](https://img.shields.io/badge/all_contributors-0-orange.svg?style=flat-square)](#contributors-)

<!-- ALL-CONTRIBUTORS-BADGE:END -->

# Curbside Pickup

This app allows shoppers and store staff to coordinate curbside pickup orders through email notifications.

1. When this app detects that a pickup order has reached the `Ready For Handling` state, it adds a comment to the order timeline stating that the curbside pickup process has begun and then sends an email to the staff of the physical store where the order will be picked up. This email contains a link that the store staff can click when the order is packed and ready to be picked up.

2. When the store staff clicks the link, this will add a comment to the order timeline and trigger a second email to be sent to the shopper, notifying them that their order is ready to be picked up. This email contains a link that the shopper can click once they have arrived at the store.

3. Once the shopper clicks the "I'm here" link in their email, this will add a comment to the order timeline and trigger a third email to be sent to the store staff informing them that the shopper has arrived and they should bring the order out to their car. This email contains a link that the store staff should click after having handed the order to the shopper.

4. When the store staff clicks the link stating that the order has been handed off, the order timeline will be updated with a comment to that effect. This completes the curbside pickup process and the order can be invoiced as usual.

## Configuration

1. [Install](https://vtex.io/docs/recipes/store/installing-an-app) `vtex.curbside-pickup` in the desired account.

2. In your admin dashboard, go to `Apps > My Apps > Curbside Pickup` to configure the app settings:

- `App Key` & `App Token`: Generate a new app key/token pair in your VTEX account (or use an existing pair) and enter them here.

3. In your admin dashboard, go to `Inventory & shipping > Pickup points`. For each of your pickup point store locations, enter the store's email address in the **Address Line 2** field. The app will send store pickup notifications to this email address.

4. In your admin dashboard, go to `Curbside Pickup` (in the sidebar menu immediately below `Inventory & shipping`). In the box titled "Almost There", click the **PROCEED** button. This will initialize the order notification hook needed by the app and will create the following email templates:

- `curbside-ready-for-packing`: Sent to the store's email address (see step 3) when a curbside order is ready for handling.
- `curbside-package-ready`: Sent to the shopper when the order is ready to be picked up.
- `curbside-at-location`: Sent to the store's email address when the shopper has arrived at the pickup location.
  ⚠️ Once the email templates are created, you may customize them as you see fit. Follow the links to the templates from the Curbside Pickup page, or navigate to `Message center > Templates` from your admin dashboard sidebar.

## Contributors ✨

Thanks goes to these wonderful people ([emoji key](https://allcontributors.org/docs/en/emoji-key)):

<!-- ALL-CONTRIBUTORS-LIST:START - Do not remove or modify this section -->
<!-- prettier-ignore-start -->
<!-- markdownlint-disable -->
<!-- markdownlint-enable -->
<!-- prettier-ignore-end -->

<!-- ALL-CONTRIBUTORS-LIST:END -->

This project follows the [all-contributors](https://github.com/all-contributors/all-contributors) specification. Contributions of any kind welcome!
