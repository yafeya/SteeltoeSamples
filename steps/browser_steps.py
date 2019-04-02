from behave import *
import command
import mechanicalsoup
import requests
import resolve
import sure
import time

@when(u'you get {url}')
def step_impl(context, url):
    url = resolve.url(context, url)
    context.log.info('getting url {}'.format(url))
    context.browser = mechanicalsoup.StatefulBrowser()
    attempt = 0
    while True:
        attempt += 1
        resp = context.browser.open(url)
        if resp.status_code < 500:
            context.log.info('GET {} [{}]'.format(url, resp.status_code))
            resp.status_code.should.equal(200)
            break
        context.log.info('failed to get {} [{}]'.format(url, resp.status_code))
        if attempt > 5:
            raise Exception('Unable to get page {} [{}]'.format(url, resp.status_code))
        time.sleep(1)

@when(u'you post "{data}" to {url}')
def step_impl(context, data, url):
    url = resolve.url(context, url)
    fields = data.split('=')
    assert len(fields) == 2, 'Invalid data format: {}'.format(data)
    payload = { fields[0]: fields[1] }
    context.log.info('posting url {} {}'.format(url, payload))
    context.browser = mechanicalsoup.StatefulBrowser()
    resp = context.browser.post(url, data=payload)
    context.log.info('POST {} [{}]'.format(url, resp.status_code))

@when(u'you login with "{username}"/"{password}"')
def step_impl(context, username, password):
    context.browser.select_form('form[action="/login.do"]')
    context.browser['username'] = username
    context.browser['password'] = password
    context.browser.submit_selected()

@then(u'you should be at {url}')
def step_impl(context, url):
    context.browser.get_url().should.match(r'{}(\?.*)?'.format(resolve.url(context, url)))

@then(u'you should see "{text}"')
def step_impl(context, text):
    context.browser.get_current_page().get_text().should.match(r'.*{}.*'.format(text))

@then(u'you should be able to access CloudFoundry app {app} management endpoints')
def step_impl(context, app):
    url = resolve.url(context, 'https://{}.x.y.z/cloudfoundryapplication'.format(app))
    token = get_oauth_token(context)
    resp = requests.get(url, headers={'Authorization': token})
    resp.status_code.should.equal(200)
    context.log.info(resp.content)
    for endpoint in ['info', 'health', 'loggers', 'trace', 'mappings']:
        resp.text.should.contain('/cloudfoundryapplication/{}'.format(endpoint))

def get_oauth_token(context):
    cmd = command.Command(context, 'cf oauth-token')
    cmd.run()
    return cmd.stdout.strip()
