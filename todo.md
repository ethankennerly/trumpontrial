# Features

Android/WebGL: <http://finegamedesign.com/trumpontrial>

1. Top/bottom ui anchors to screen top/bottom.
1. Speech bubble displays longest trial text.
1. Parse trials from CSV.
1. Scroll prompts to identify argument.
1. Tap button submits option in range.
1. Correct answer corresponds to answer key.
1. Button on each option.
1. Submit button appears to enable selection.
1. Snap scroll to first content on argument populated.
1. If wrong, animate text of Trump saying "WRONG."
1. During feedback, hide input and argument.
1. After feedback, then show next argument.
1. If correct, animate Trump saying "True. But..."
1. Sort arguments by estimated difficulty.
1. On Answer, hear button sound.
1. If correct, hear correct sound.
1. If wrong, hear different sound.
1. Goto progress animator increments when correct.
1. Wrong answer increments opponent progress bar.
1. Parser inspector overrides number of arguments.
1. After a number of arguments, end arguments.
1. Results screen displays accuracy percent.
1. Dial on results to replay.
1. Title screen: Trump on Trial.
1. Options: Start. Uses same dial as trials.
1. Each correct answer increases number of fallacies.
1. Clamp content when fewer options.
1. Blue cartoon UI button style.
1. Blue cartoon UI font: Life is goofy.
1. Three stars on win meter.
1. Identify 3 easy fallacies.
1. Identify 15 hard fallacies.
1. Linear interpolation on progress bar timelines.
1. Option prompt off tall screen in feedback.
1. Increase button click height to 80 design pixels.
1. Remove hammer.
1. Remove unused option "Sound".
1. Prompt "I am ready to..."
1. Scroll touch on left and right of buttons.
1. Trump as Android icon and round icon.
1. Event System: Large drag threshold for Android.
1. Report first interaction event.
1. Report level index started.
1. Report argument as level complete.
1. Report argument as level fail with wrong answer.
1. Prompt: "(Drag up, then tap one)"
1. No assertion of unreceived animation event.
1. Clear compiler warnings of [unassigned variables.](warning_c0649_field_is_never_assigned.md)
1. Argument Viewer logs optional and off by default.
1. No static non-delegate fields.
1. Argument Viewer: No static fields.
1. Fallacy Option Viewer: No static fields.
1. Answer Feedback Publisher: No static fields.
1. Session Performance Animator: No static fields.
1. Argument Evaluate: Extracts from Argument Viewer.
1. Fallacy Lister: Independent of parser.
1. Distractor Adjuster: Bridges argument and fallacy.
1. Argument/Fallacy: Wraps delegate assignments.
1. Arguments Table Tests: Parses tiny CSV.
1. Arguments Table: Extracts from parser Behaviour.
1. Fallacies Table: Extracts from parser Behaviour.
1. Arguments / Fallacies Tables: Caches parsing.
1. Namespace "Argument" to "FallacyRecognition".
1. Argument Evaluator Tests: Evaluates fallacy.
1. Fallacy Recognition Session Tests: Stubs.
1. Fallacy Parser Tests: Parses from prefab.
1. Argument Parser Tests: Parses from prefab.
1. Fallacy Recognition Session Tests: Setup.

# Nice to have

1. [ ] Argument Fallacy Bridge Tests: Renames.
1. [ ] Argument Fallacy Bridge Tests: All args.
1. [ ] Argument Fallacy Bridge: Integrates.
1. [ ] On results, Trump says impeached/you're fired.
1. [ ] Option to link to read intro to fallacies.
1. [ ] When pass star, that color highlights star.
1. [ ] If correct, correct icon flies to meter.
1. [ ] Introduce progress bar with first argument.
1. [ ] If wrong, wrong icon flies to meter.
1. [ ] If correct, animate shuffled text.
1. [ ] If wrong, animate shuffled text.
1. [ ] On select, animate hammer.
1. [ ] On select, animate stamp.
1. [ ] Gradient over scroll.
1. [ ] Scroll contents distort as if on a barrel.
1. [ ] Hear background music.
1. [ ] Animation shows Manaford, Stone, Trump.
1. [ ] Progress shows Manaford, Stone, Trump impeached.
1. [ ] If correct, animate Trump speech shattering.
1. [ ] Estimate difficulty of recognizing fallacy.
1. [ ] If wrong, explain fallacy.
1. [ ] Tap accelerates animator until end of anim.
1. [ ] Slider setting speeds up time scale.
1. [ ] At top left, read countdown timer.
1. [ ] After time up, end game.
1. [ ] When time almost up, highlight timer.
1. [ ] Validate correct answer exactly once in options.
1. [ ] If wrong, animate bottom speech shattering.
1. [ ] Results screen displays time.
1. [ ] Answer feedback pauses timer.
1. [ ] Timer shows: November 2016 to November 2020.
1. [ ] When time almost up, hear sound.
1. [ ] Countdown 3-2-1-GO.
1. [ ] How can Jen select an option?
1. [ ] When mouse up, drift to snap scroll to option.
1. [ ] Keyboard arrows, or input axis, scrolls options.
1. [ ] Enter key or space bar submits.
