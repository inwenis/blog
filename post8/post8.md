# node packages updating

Need to update dependencies in a node js project? Here are my notes on this.

`> npm i` (`npm install`) cleans+installs+updsets `node_modules` + shows what is going on with packaged in your project

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
- installs missing packages
- removes redundant packages
- installs correct versions of mismatched packages (if `packages-lock.json` wants a different version than found in `node_modules`).

`> npm audit` - shows a report on venerability issues in your dependencies.

`> npm audit fix` - updates packages to address issues (updates that do not require attention)

`> npm outdated` - shows a table with your packages and version
```
$ npm outdated
Package      Current   Wanted   Latest  Location                  Depended by
glob          5.0.15   5.0.15    6.0.1  node_modules/glob         dependent-package-name
nothingness    0.0.3      git      git  node_modules/nothingness  dependent-package-name
npm            3.5.1    3.5.2    3.5.1  node_modules/npm          dependent-package-name
local-dev      0.0.3   linked   linked  local-dev                 dependent-package-name
once           1.3.2    1.3.3    1.3.3  node_modules/once         dependent-package-name
```

- Current - what is in `nodes_modules`
- Wanted - most recent version that respect the version constraint from `packages.json`
- Latest - latest version from npm registry

If you want to update to latest minor+patch versions of your dependencies you see all required info with `npm outdated` but I prefer the output of `npm-check-updates`

`> npm i npm-check-updates -g` (-g -> global mode - package will be available on your whole machine)


`> npm check-updates` - using colors nicely shows where an update will be a major or minor or patch version update

# Let's finally update something

`> npm update` (perform updates respecting your semver constraints and update `package-lock.json`)
`> npm update --save` (same as above but also update `packages.json`, use this one always)

The bevaviour for packages with major version 0.*.* is different than for versions >=`1.0.0` (see `npm help update`)

`npm update` will most likely bump all minor and patch versions for you.

You can run `npm update --save` often.

# What do the symbols in `package.json mean?`
https://stackoverflow.com/questions/22343224/whats-the-difference-between-tilde-and-caret-in-package-json/25861938#25861938

# `npm update --save` vs `npm audit fix`
`npm audit fix` will only update packages to fix vunerability issues
`npm update --save` will update all packages it can (respscting semver constraints)

# Do I have unused dependencies?
`> npm install depcheck -g`

`> depcheck`

Shows you unused dependencies. Beware that it just scans for require/import statements in your code so you might be using a package in a different way and `depcheck` will think it is unused.
(example - when you import packages using `importLazy`)

---------

`> npm i npm-check -g`

`> npm-check`

Another tool to help with dependencies, didn't use it.

`> npm ls` - list installed packages

`> npm ls axios` - show all version of axios and why we have them


# That's all cool, how do i update packages?
1. `> npm i` - make order in `node_modules`
1. `> npm audit` - see your venerability issues
1. `> npm audit fix` - fix venerability issues that don't require attention
1. Create a pull-request here (fix-venerability-issues)
---
1. `> npm i npm-check-updates -g`
1. `> npm-check-updates` - see how outdated your packages are
1. `> npm outdated` - see how outdated your packages are
1. `> npm update --save` - update packages respecting your semver constraints from `packages.json`
1. If you have packages that use major version `0.*.*` you'll need to manually update there here:
1. `> npm install that-one-package@latest`
1. Create a pull-request here (update-packages-minor)
---
1. If you're brave and can test your project you can
1. `ncu -u` - updates `packages.json` to all latest versions as shown by `npm-check-updates`
    - This might introduce breaking changes.
1. `npm i`
1. Test your project.
1. Create pull-request
---
1. If you're not brave or can't just YOLO and update all major versions:
1. `npm-check-updates` - check again what is left to update
1. `npm i that-package@latest` - make a major update of `that-package`
1. Test your project, Since .js is dynamically typed you might have just updated a package that breaks your project but you'll not know until you run your code.
1. Repeat for all packages.

