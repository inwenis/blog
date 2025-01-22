---
draft: false
date: 2024-06-17
categories:
  - General coding
  - Team lead
---

# It will be great, set it up, don't use it, remove it

Today, I removed something I allowed to be created despite initially feeling it was redundant.

A year ago, a student found a tool to auto-generate documentation for our internal SDK from code annotations. They proposed embedding this documentation in our Continuous Integration pipeline. Although the idea sounded good on paper, I felt it wouldn’t be used by our team. However, the team was enthusiastic.

Everyone in our team has the SDK's git repo on their machine, and we rely on IntelliSense and the code for documentation. It seemed unlikely that we would change our habits since the new documentation wouldn’t be easier to use than just using F12 to view the source code.

Despite my doubts, I allowed this feature as the team lead. I had already rejected a few initiatives from that student and didn’t want to kill their motivation. I wanted to let them work on something they found interesting. There was a slight chance I was wrong and the documentation might be used.

Today, a year later, I noticed the docs website no longer works. It had been down for some time, and no one noticed because no one used it. I removed any trace of the published documentation.

This made me reflect: was it wrong to allow something to be created that never paid off?

In this case, the investment was small. The gain was that the student got to work on something interesting. So, I think it was right to let our team try it out and remove it once we were certain it wasn’t used