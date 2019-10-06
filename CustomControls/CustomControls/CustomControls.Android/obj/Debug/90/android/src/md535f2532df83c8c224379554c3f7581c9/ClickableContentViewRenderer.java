package md535f2532df83c8c224379554c3f7581c9;


public class ClickableContentViewRenderer
	extends md51558244f76c53b6aeda52c8a337f2c37.VisualElementRenderer_1
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onTouchEvent:(Landroid/view/MotionEvent;)Z:GetOnTouchEvent_Landroid_view_MotionEvent_Handler\n" +
			"";
		mono.android.Runtime.register ("CustomControls.Droid.ClickableContentViewRenderer, CustomControls.Android", ClickableContentViewRenderer.class, __md_methods);
	}


	public ClickableContentViewRenderer (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == ClickableContentViewRenderer.class)
			mono.android.TypeManager.Activate ("CustomControls.Droid.ClickableContentViewRenderer, CustomControls.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public ClickableContentViewRenderer (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == ClickableContentViewRenderer.class)
			mono.android.TypeManager.Activate ("CustomControls.Droid.ClickableContentViewRenderer, CustomControls.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public ClickableContentViewRenderer (android.content.Context p0)
	{
		super (p0);
		if (getClass () == ClickableContentViewRenderer.class)
			mono.android.TypeManager.Activate ("CustomControls.Droid.ClickableContentViewRenderer, CustomControls.Android", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public boolean onTouchEvent (android.view.MotionEvent p0)
	{
		return n_onTouchEvent (p0);
	}

	private native boolean n_onTouchEvent (android.view.MotionEvent p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
