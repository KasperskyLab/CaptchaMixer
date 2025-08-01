# Security considerations

## Automated recognition

We've conducted a study on the complexity of automating image recognition using machine learning.
The result is that once you've collected enough image examples (10-100k) you may achieve up to 90% of successful recognition rate.
The level of expertise required for utilizing some popluar open-sourced machine learning engine appeared to be pretty low - something an average intern may handle.

Of course, our research had significant specifics such as CAPTCHA text length, its characters set, used image sizes, and even the fact itself that our CAPTCHAs were text-image ones.
So results in other contexts may significantly differ.
But regarding text-image CAPTCHAs we anyway believe that the task of automating their recognition is overall simpler than one may think it is.

## Do not limit yourself to text-image CAPTCHAs

CaptchaMixer has been originally designed for text-image CAPTCHAs and we still use it that way as our backup CAPTCHA option in situations when external CAPTCHA providers cannot be used.
The problem is that good old text-image CAPTCHA is a **primitive protection technique** which nowadays is **almost an archaism**.
The trend of recent years is CAPTCHA tests which require more complex user interaction or recognition of something else then just text.

Can CaptchaMixer help you with that?
Yes, because in fact it is a general purpose image rendering engine with just some CAPTCHA-specific additions.
The CAPTCHA `answer` is internally a `string` but nothing limits you in terms of how you interpret it.
For example, it may contain a JSON describing a labyrinth which you then render on the image and ask the user to walk it through with his mouse (on PCs) or his fingers (on mobiles).

## Enhance your security beyond just CAPTCHAs

No CAPTCHAs can protect your services against well-organized attacks.
No matter how resistant your CAPTCHA images to automated recognition are when a serious attacker may hire a CAPTCHA solving service where real people will be doing the job.
There are many of them, just take a look.

Using external CAPTCHA services such as reCAPTCHA or hCaptcha will lower some of the risks due to multiple smart techniques they use to recognize malicious traffic, but even they cannot prevent all possible attacks at once.

So your services shall also have an additional layer of security specific to their business logic.
Rate-limiting crucial and heavy operations, various flood and bruteforce protections, IP address filtering - just some of the things you may come up with.
Be creative, invent and combine protection methods.