import React, { FC } from 'react'
import { Layout, PageHeader, Box, Link, IconCog } from 'vtex.styleguide'
import { injectIntl, FormattedMessage } from 'react-intl'

const getAppId = () => {
  return process.env.VTEX_APP_ID || ''
}

const CurbsideIndex: FC = ({ intl }: any) => {
  return (
    <Layout
      pageHeader={
        <div className="flex justify-center">
          <div className="w-100 mw-reviews-header">
            <PageHeader
              title={intl.formatMessage({ id: 'admin/curbside.title' })}
            >
              <Link href={`/admin/apps/${getAppId()}/setup`}>
                <IconCog size="16" /> <FormattedMessage id="admin/curbside.settings.title" />
              </Link>
            </PageHeader>
          </div>
        </div>
      }
      fullWidth
    >
      <div className="bg-muted-5 pa4 w-50">
        <Box
          title={intl.formatMessage({ id: 'admin/curbside.templates.title' })}
          fit="none"
        >
          <ul className="mid-gray">
            <li className="mb4">
              <a href="/admin/message-center/#/templates/curbside-at-location">
                <FormattedMessage id="admin/curbside.template.at-location" />
              </a>
            </li>
            <li className="mb4">
              <a href="/admin/message-center/#/templates/curbside-cancel-order">
                <FormattedMessage id="admin/curbside.template.cancel-order" />
              </a>
            </li>
            <li className="mb4">
              <a href="/admin/message-center/#/templates/curbside-package-ready">
                <FormattedMessage id="admin/curbside.template.package-ready" />
              </a>
            </li>
            <li className="mb4">
              <a href="/admin/message-center/#/templates/curbside-ready-for-packing">
                <FormattedMessage id="admin/curbside.template.ready-for-packing" />
              </a>
            </li>
          </ul>
        </Box>
      </div>
    </Layout>
  )
}

export default injectIntl(CurbsideIndex)
