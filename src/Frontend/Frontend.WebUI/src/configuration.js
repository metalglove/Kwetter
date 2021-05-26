export default class Configuration {
  static get EnvConfig () {
    return {
      VUE_APP_APIKEY: '$VUE_APP_APIKEY',
      VUE_APP_AUTH_DOMAIN: '$VUE_APP_AUTH_DOMAIN',
      VUE_APP_PROJECT_ID: '$VUE_APP_PROJECT_ID',
      VUE_APP_STORAGE_BUCKET: '$VUE_APP_STORAGE_BUCKET',
      VUE_APP_MESSAGING_SENDER_ID: '$VUE_APP_MESSAGING_SENDER_ID',
      VUE_APP_APP_ID: '$VUE_APP_APP_ID',
      VUE_APP_GATEWAY_API_URL: '$VUE_APP_GATEWAY_API_URL',
      VUE_APP_GATEWAY_WS_API_URL: '$VUE_APP_GATEWAY_WS_API_URL'
    }
  }

  static value (key) {
    // If the key does not exist in the EnvConfig object of the class, return null
    if (!this.EnvConfig.hasOwnProperty(key)) {
      console.error(`Configuration: There is no key named "${key}". Please add it in Configuration class.`)
      return
    }

    // Get the value
    const value = this.EnvConfig[key]

    // If the value is null, return
    if (!value) {
      console.error(`Configuration: Value for "${key}" is not defined`)
      return
    }

    if (!value.startsWith('$VUE_APP_')) {
      // value was already replaced, it seems we are in production (containerized).
      return value
    }

    // value was not replaced, it seems we are in development.
    const envName = value.substr(1) // Remove $ and get current value from process.env
    const envValue = process.env[envName]

    if (!envValue) {
      console.error(`Configuration: Environment variable "${envName}" is not defined`)
      return
    }

    return envValue
  }
}
