import React, { FC } from 'react'
import {
  Layout,
  PageHeader,
} from 'vtex.styleguide'

const getAppId = () => {
  return process.env.VTEX_APP_ID
}

const CurbsideIndex: FC = props => {
  return (
    <Layout
      pageHeader={
        <div className="flex justify-center">
          <div className="w-100 mw-reviews-header">
            <PageHeader
              title="Curbside Pickup"
             />
          </div>
        </div>
      }
      fullWidth
    >
        <div className="flex justify-center">
          <div className="w-100 mw-reviews-content pb8">
            <div className="mb7">
              <ul>
                <li><a href="#">Thanks email template</a></li>
                <li><a href="#">Handover email template</a></li>
                <li><a href="#">Notified email template</a></li>
                <li><a href="#">Cancelled email template</a></li>
                <li><a href={`/admin/apps/${getAppId()}/setup`}>App settings</a></li>
              </ul>
            </div>
            {props.children}
          </div>
        </div>
    </Layout>
  )
}

export default CurbsideIndex
