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

2. In your admin dashboard, go to `Apps > Curbside Pickup` to configure the app settings:

- TBD

## Contributors ✨

Thanks goes to these wonderful people ([emoji key](https://allcontributors.org/docs/en/emoji-key)):

<!-- ALL-CONTRIBUTORS-LIST:START - Do not remove or modify this section -->
<!-- prettier-ignore-start -->
<!-- markdownlint-disable -->
<!-- markdownlint-enable -->
<!-- prettier-ignore-end -->

<!-- ALL-CONTRIBUTORS-LIST:END -->

This project follows the [all-contributors](https://github.com/all-contributors/all-contributors) specification. Contributions of any kind welcome!
