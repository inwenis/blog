---
draft: false
date: 2024-05-09
categories:
  - node
  - npm
  - js
  - dependencies
---

# node packages updating

## tl;dr
1. `> npm install depcheck -g` - install `depcheck` globally
1. `> depcheck` - check for redundant packages
1. `> npm un this-redundant-package` - uninstall redundant packages (repeat for all redundant packages)
1. Create a pull-request `remove-redundant-packages`
---
1. `> npm i` - make order in `node_modules`
1. `> npm audit` - see vulnerability issues
1. `> npm audit fix` - fix vulnerability issues that don't require attention
1. Create a pull-request `fix-vulnerability-issues`
---
1. `> npm i npm-check-updates -g` - install `npm-check-updates` globally
1. `> npm-check-updates` - see how outdated packages are
1. `> npm outdated` - see how outdated packages are
1. `> npm update --save` - update packages respecting your semver constraints from `packages.json`
1. If you have packages that use major version `0.*.*` you'll need to manually update these now
    - `> npm install that-one-package@latest`
1. Create a pull-request `update-packages-minor`
---
If you're brave and can test/run you project easily:

1. `ncu -u` - updates `packages.json` to all latest versions as shown by `npm-check-updates`
    - this might introduce breaking changes
1. `npm i` - update `package-lock.json`
1. Test your project.
1. Create a pull-request `update-packages-major`
---
If you're not brave or can't just YOLO and update all major versions:

1. `npm-check-updates` - check again what is left to update
1. `npm i that-package@latest` - update major version of of `that-package`
1. Test your project.
    - .js is dynamically typed so you might have just updated a package that breaks your project but you'll not know until you run your code
1. Repeat for all packages.
1. Create a pull-request `update-packages-major`

## longer read

Need to update dependencies in a node js project? Here are my notes on this.

`> npm i` (`npm install`)
```
> npm i

added 60 packages, removed 124 packages, changed 191 packages, and audited 522 packages in 13s

96 packages are looking for funding
  run `npm fund` for details

10 vulnerabilities (2 low, 7 moderate, 1 high)

To address issues that do not require attention, run:
  npm audit fix

To address all issues possible (including breaking changes), run:
  npm audit fix --force

Some issues need review, and may require choosing
a different dependency.

Run `npm audit` for details.
```
- installs missing packages in `node_modules`
- removes redundant packages in `node_modules`
- installs correct versions of mismatched packages (if `packages-lock.json` wants a different version than found in `node_modules`)
- shows what is going on with packaged in your project

`> npm audit` - shows a report on vulnerability issues in your dependencies

`> npm audit fix` - updates packages to address vulnerability issues (updates that do not require attention)

`> npm outdated` - shows a table with your packages and versions
```
$ npm outdated
Package      Current   Wanted   Latest  Location                  Depended by
glob          5.0.15   5.0.15    6.0.1  node_modules/glob         dependent-package-name
nothingness    0.0.3      git      git  node_modules/nothingness  dependent-package-name
npm            3.5.1    3.5.2    3.5.1  node_modules/npm          dependent-package-name
local-dev      0.0.3   linked   linked  local-dev                 dependent-package-name
once           1.3.2    1.3.3    1.3.3  node_modules/once         dependent-package-name
```

- `Current` - what is in `nodes_modules`
- `Wanted` - most recent version that respect the version constraint from `packages.json`
- `Latest` - latest version from npm registry

To update to latest minor+patch versions of your dependencies (`Wanted`) - `npm outdated` shows all you need to know but I prefer the output of `npm-check-updates`

`> npm i npm-check-updates -g` (`-g` -> global mode - package will be available on your whole machine)

`> npm-check-updates` - shows where an update will be a major/minor/patch update (I like the colors)
```
Checking C:\git\blog\package.json
[====================] 39/39 100%

 @azure/storage-blob         ^12.5.0  →      ^12.17.0
 adm-zip                     ^0.4.16  →       ^0.5.12
 axios                       ^0.27.2  →        ^1.6.8
 basic-ftp                    ^5.0.1  →        ^5.0.5
 cheerio                 ^1.0.0-rc.6  →  ^1.0.0-rc.12
 eslint                      ^8.12.0  →        ^9.2.0
 eslint-config-prettier       ^8.5.0  →        ^9.1.0
 eslint-plugin-import        ^2.25.4  →       ^2.29.1
 fast-xml-parser              ^4.2.4  →        ^4.3.6
 humanize-duration           ^3.27.3  →       ^3.32.0
 iconv                        ^3.0.0  →        ^3.0.1
 jsonwebtoken                 ^9.0.0  →        ^9.0.2
 luxon                        ^3.4.3  →        ^3.4.4
```
### Let us update something

`> npm update` - perform updates respecting your semver constraints and update `package-lock.json`

`> npm update --save` - same as above but also update `packages.json`, use this one always

**The behavior for packages with major version `0.*.*` is different than for versions >=`1.0.0` (see `npm help update`)**

`npm update` will most likely bump all minor and patch versions for you.

You can run `npm update --save` often.

### What do the symbols in `package.json` mean?
https://stackoverflow.com/questions/22343224/whats-the-difference-between-tilde-and-caret-in-package-json/25861938#25861938

### `npm update --save` vs `npm audit fix`
`npm audit fix` will only update packages to fix vulnerability issues

`npm update --save` will update all packages it can (respecting semver constraints)

### Do I have unused dependencies?
`> npm install depcheck -g`

`> depcheck` - shows unused dependencies.
`depcheck` scans for `require`/`import` statements in your code so you might be utilizing a package differently but `depcheck` will consider it unused (ex. when you import packages using `importLazy`).

### npm-check

`> npm i npm-check -g`

`> npm-check` - a different tool to help with dependencies (I didn't use it)

### honorable mentions

`> npm ls` - list installed packages (from `node_modules`)

`> npm ls axios` - show all versions of axios and why we have them

`npm ls` will not show you origin of not-installed optional dependencies.

Consider this - you devleop on a `win` maching and deploy your solution to a `linux` box.
On windows (see below) you might think `node-gyp-build` is not used in your solution.
```
> npm ls node-gyp-build
test-npm@1.0.0 C:\git\test-npm
`-- (empty)
```

But on a linux box it will be used:
```
> npm ls node-gyp-build
npm-test-proj@1.0.0 /git/npm-test-proj
└─┬ kafka-lz4-lite@1.0.5
  └─┬ piscina@3.2.0
    └─┬ nice-napi@1.0.2
      └── node-gyp-build@4.8.1
```
