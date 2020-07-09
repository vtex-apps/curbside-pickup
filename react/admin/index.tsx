import React, { FC, useState, useEffect } from 'react'
import {
  Layout,
  PageHeader,
  Box,
  IconCog,
  Card,
  Spinner,
  Button,
  Tag,
} from 'vtex.styleguide'
import { injectIntl, FormattedMessage } from 'react-intl'
import { useQuery } from 'react-apollo'
import AppSettings from '../graphql/AppSettings.graphql'

const getAppId = () => {
  return process.env.VTEX_APP_ID || ''
}
let verified = false

const CurbsideIndex: FC = ({ intl }: any) => {
  const { data, loading } = useQuery(AppSettings, {
    variables: {
      version: process.env.VTEX_APP_VERSION,
    },
    ssr: false,
  })

  const [state, setState] = useState<any>({
    checkLoading: true,
    isSetup: false,
    setupLoading: false,
    error: false,
    errorMessage: null,
  })

  const { checkLoading, isSetup, setupLoading, error, errorMessage } = state

  const verify = () => {
    verified = true
    fetch('/_v/curbside-pickup/verify-setup')
      .then(res => res.json())
      .then(
        result => {
          console.log('Result check =>', result)
          setState({
            ...state,
            checkLoading: false,
            isSetup: result,
          })
        },
        error => {
          console.log('Result check error =>', error)
          setState({
            ...state,
            checkLoading: false,
            error: true,
            errorMessage: error,
          })
        }
      )
  }

  useEffect(() => {
    if (!verified) {
      verify()
    }
  })

  const setup = () => {
    setState({
      ...state,
      setupLoading: true,
    })
    fetch('/_v/curbside-pickup/initialize-app')
      .then(res => res.json())
      .then(
        result => {
          console.log('Result setup =>', result)
          setState({
            ...state,
            checkLoading: false,
            setupLoading: false,
            isSetup: true,
          })
        },
        error => {
          console.log('Result setup error =>', error)
          setState({
            checkLoading: false,
            setupLoading: false,
            error: true,
            errorMessage: error,
          })
        }
      )
  }

  console.log('appSettings', data)

  return (
    <Layout
      pageHeader={
        <div className="flex justify-center">
          <div className="w-100 mw-reviews-header">
            <PageHeader
              title={intl.formatMessage({ id: 'admin/curbside.title' })}
            >
              <a href={`../app/apps/${getAppId()}/setup`} target="_self">
                <IconCog size="16" />{' '}
                <FormattedMessage id="admin/curbside.settings.title" />
              </a>
            </PageHeader>
          </div>
        </div>
      }
      fullWidth
    >
      <div className="bg-muted-5 pa4 w-70-m w-50-l w-100-s">
        {error && (
          <Tag type="error" variation="low">
            {errorMessage}
          </Tag>
        )}
        {(loading || checkLoading) && <Spinner />}
        {!loading && !checkLoading && !data?.appSettings?.message && (
          <div>
            <Card>
              <h2>
                <FormattedMessage id="admin/curbside.warning.title" />
              </h2>
              <p>
                <FormattedMessage id="admin/curbside.warning.description" />{' '}
                <a
                  href="https://developers.vtex.com/docs/getting-started-authentication#creating-the-appkey-and-apptoken"
                  target="_blank"
                >
                  <FormattedMessage id="admin/curbside.warning.link" />
                </a>
              </p>
            </Card>
          </div>
        )}
        {!loading && !checkLoading && data?.appSettings?.message && !isSetup && (
          <div>
            <Card>
              <h2>
                <FormattedMessage id="admin/curbside.setup.title" />
              </h2>
              <p>
                <FormattedMessage id="admin/curbside.setup.description" />{' '}
                <div className="mt4">
                  <Button
                    variation="primary"
                    isLoading={setupLoading}
                    collapseLeft={true}
                    onClick={() => {
                      setup()
                    }}
                  >
                    <FormattedMessage id="admin/curbside.setup.button" />
                  </Button>
                </div>
              </p>
            </Card>
          </div>
        )}
        {!loading && !checkLoading && data?.appSettings?.message && isSetup && (
          <Box
            title={intl.formatMessage({ id: 'admin/curbside.templates.title' })}
            fit="none"
          >
            <ul className="mid-gray">
              <li className="mb4">
                <a
                  href="/admin/message-center/#/templates/curbside-ready-for-packing"
                  target="_top"
                >
                  <FormattedMessage id="admin/curbside.template.ready-for-packing" />
                </a>
              </li>
              <li className="mb4">
                <a
                  href="/admin/message-center/#/templates/curbside-package-ready"
                  target="_top"
                >
                  <FormattedMessage id="admin/curbside.template.package-ready" />
                </a>
              </li>
              <li className="mb4">
                <a
                  href="/admin/message-center/#/templates/curbside-at-location"
                  target="_top"
                >
                  <FormattedMessage id="admin/curbside.template.at-location" />
                </a>
              </li>
            </ul>
          </Box>
        )}
      </div>
    </Layout>
  )
}

export default injectIntl(CurbsideIndex)
