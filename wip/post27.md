auto formatting:
- brings consistency

consistency != readability
    tho they overlap to some extend

consistency is only important as long as it improves readability
    we already break consistency in several places to improve readability

https://luminousmen.com/post/my-unpopular-opinion-about-black-code-formatter/
Black is a cargo cult
    do we format code cuz it adds value
    or do we format code cuz good teams have their code formatted

https://peps.python.org/pep-0008/
However, know when to be inconsistent – sometimes style guide recommendations just aren’t applicable. When in doubt, use your best judgment. Look at other examples and decide what looks best. And don’t hesitate to ask

I love tabular stuff

auto formatting:
- removes ego? - it's repeated often, not sure if it's true
- easier diff - yes, but we can also stick to our recommended formatting
 - easier merges (consequence of easier diff)
 - easier history browsing (consequence of easier diff) (do we do that?)
 - but fantomas enforces formatting that is worst at diffs than Peder's guide
- standards settle disputes (do we have disputes, will we have them?)

There is a difference between "style guide" and "strictly enforced code format"

well formatted is easier to read
  but is auto-formatted well-formatted?



# Discussion on F# Formatting

## Agreement

1. **Default Formatter**
   We will use *Fantomas* as the default code formatter.

2. **Permitted Deviations**
   Deviations from Fantomas are allowed when readability requires it.
   - We may exclude specific files from Fantomas checks without hesitation.

3. **Style Guide Consistency**
   Our internal style guide (i.e., the list of permitted deviations from Fantomas) will not differ drastically from the Fantomas standard.
   - This is to reduce cognitive overhead when switching between codebases using standard Fantomas formatting and those with our customized rules.

> **Note:** The location and format of our style guide have not yet been determined. This will be resolved over time.
