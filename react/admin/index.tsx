import React, { FC } from 'react'
import {
  Layout,
  PageHeader,
} from 'vtex.styleguide'



const CurbsideIndex: FC = props => {

  return (
    <Layout
      pageHeader={
        <div className="flex justify-center">
          <div className="w-100 mw-reviews-header">
            <PageHeader
              title="Curbside/Pickup"
             />
          </div>
        </div>
      }
      fullWidth
    >
        <div className="flex justify-center">
          <div className="w-100 mw-reviews-content pb8">
            <div className="mb7">
              Test
            </div>
            {props.children}
          </div>
        </div>
    </Layout>
  )
}

export default CurbsideIndex
